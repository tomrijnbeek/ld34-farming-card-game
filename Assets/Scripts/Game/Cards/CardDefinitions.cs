using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class CardDefinitions : Singleton<CardDefinitions> {

    public enum CardTypes {
        Produce = 0,
        Effect = 1,
        Action = 2,
    }

    public Color[] backColors;
    public Color[] textColors;

    public GameObject plantPrefab;

    public CardDefinition[] cards;

    public SpriteDefinition[] spritesets;
    Dictionary<string, Sprite[]> spriteDict = new Dictionary<string, Sprite[]>();

    void Start () {
        foreach (var ss in spritesets)
            spriteDict[ss.name] = ss.sprites;

        cards = new [] {
            #region Produce
            Plant<Plant>("Basic plant", "", "plant", 9, 0, 5, 1),

            Plant<Plant>("Large plant", "Reduces growth in neighbouring tiles by 50%.", "plant", 7, 0, 10, 1,
                go => NeighbourEffect(go, GrowthInfluence("Large plant adjacency", -.5f))
            ),

            Plant<Plant>("Recyclable plant", "Leaves the tile composted after growing", "plant", 11, 2, 6, 1,
                go => SelfEffect(go, new EffectDefinition() {
                    Do = tiles => { },
                    Undo = Compost(),
                })
            ),

            Plant<Mushrooms>("Mushrooms", "Not affected by growth bonuses, but grows 50% in shadow.", "mushrooms",
                15, 0, 12, 1),
            #endregion

            #region Effect
            Effect("Rain shower", "Accelerates growth by 50%.", 2, GrowthInfluence("Rain shower", .5f), 10, 1),

            Effect("Sprinkler", "Accelerates growth in 3x3 area by 100%.", 4, GrowthInfluence("Sprinkler", 1), 10, 1, 3, 3),
            #endregion

            #region Action
            Action("Compost", "Compost all empty tiles in a 2x2 area", Compost(), 3, 1, 2, 2, AnyEmpty()),

            Action("Dirty landry", "Discards entire hand.", t => Hand.Instance.DiscardAll(), 0, 1),

            Action("Growing spurt", "Advances all procude one extra step.", GrowthStep(1), 5, 1),

            Action("Temporal anomaly", "Instantly finishes a produce.", GrowthStep(1000), 10, 1, 1, 1, AllProduce()),
            #endregion
        };
    }

    CardDefinition Plant<T>(string name, string description, string spriteSet, int maxProgress,
            int cost, int gain, float w, Action<GameObject> specialStuff = null)
            where T : Plant, new() {
        return new CardDefinition() {
            name = name,
            description = description,
            type = CardTypes.Produce,
            cost = cost,
            turns = maxProgress,
            gain = gain,
            pWeight = w,
            areaWidth = 1,
            areaHeight = 1,
            Do = tiles => {
                if (tiles.Length != 1)
                    throw new UnityException("Unexpected number of tiles.");

                var obj = Instantiate(plantPrefab);

                var plant = obj.AddComponent<T>();
                plant.title = name;
                plant.sprites = spriteDict[spriteSet];
                plant.maxProgress = maxProgress;
                plant.score = gain;

                if (specialStuff != null)
                    specialStuff(obj);

                obj.transform.parent = tiles[0].transform;
                obj.transform.localPosition = Vector3.zero;
            },
            AreaCheck = AllEmpty(),
        };
    }

    CardDefinition Effect(string name, string description, int duration, EffectDefinition action,
            int cost, float w, int areaW = 0, int areaH = 0, Predicate<Tile[]> areaCheck = null) {
        return new CardDefinition() {
            name = name,
            description = description,
            type = CardTypes.Effect,
            cost = cost,
            turns = duration,
            pWeight = w,
            areaWidth = areaW,
            areaHeight = areaH,
            AreaCheck = areaCheck,
            Do = tiles => {
                EffectManager.Instance.AddEffect(new Effect(name, action, duration, tiles));
            }
        };
    }

    CardDefinition Action(string name, string description, Action<Tile[]> action,
            int cost, int w, int areaW = 0, int areaH = 0, Predicate<Tile[]> areaCheck = null) {
        return new CardDefinition() {
            name = name,
            description = description,
            type = CardTypes.Action,
            cost = cost,
            pWeight = w,
            areaWidth = areaW,
            areaHeight = areaH,
            AreaCheck = areaCheck,
            Do = action
        };
    }

    void NeighbourEffect(GameObject go, EffectDefinition action) {
        var eff = go.AddComponent<NeighbourEffect>();
        eff.action = action;
    }

    void SelfEffect(GameObject go, EffectDefinition action) {
        var eff = go.AddComponent<SelfEffect>();
        eff.action = action;
    }

    EffectDefinition GrowthInfluence(string desc, float amount) {
        var influence = new GrowthRateInfluence() {
            amount = amount,
            description = desc,
        };
        return new EffectDefinition() {
            Do = tiles => {
                foreach (var t in tiles)
                    t.AddInfluence(influence);
            },
            Undo = tiles => {
                foreach (var t in tiles)
                    t.EndInfluence(influence);
            }
        };
    }

    Action<Tile[]> Compost() {
        return tiles => {
            foreach (var t in tiles) {
                if (t.plant == null)
                    t.Compost();
            }
        };
    }

    Action<Tile[]> GrowthStep(float factor, bool ignoreRate = true) {
        return tiles => {
            foreach (var t in tiles) {
                t.DoGrowthStep(factor, ignoreRate);
            }
        };
    }

    Predicate<Tile[]> AllProduce() {
        return tiles => tiles.All(t => t.plant != null);
    }
    Predicate<Tile[]> AnyEmpty() {
        return tiles => tiles.Any(t => t.plant == null);
    }
    Predicate<Tile[]> AllEmpty() {
        return tiles => tiles.All(t => t.plant == null);
    }
}

[Serializable]
public class CardDefinition {
    public CardDefinitions.CardTypes type;
    public string name;
    public string description;
    public int cost;
    public int turns;
    public int gain;
    public float pWeight;
    public int areaWidth;
    public int areaHeight;

    public Action<Tile[]> Do;
    public Predicate<Tile[]> AreaCheck;
}

public class EffectDefinition {
    public Action<Tile[]> Do;
    public Action<Tile[]> Undo;
}

[Serializable]
public class SpriteDefinition {
    public string name;
    public Sprite[] sprites;
}
