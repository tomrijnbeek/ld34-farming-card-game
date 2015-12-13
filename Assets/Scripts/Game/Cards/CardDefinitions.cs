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
            Plant<Plant>("Basic plant", "", "plant", 12, 0, 4, 10),

            Plant<Plant>("White lily", "Prevents weeds from spawning in adjacent tiles.", "plant", 8, 2, 6, 3,
                go => NeighbourEffect(go, TileEffect("White lily protection", Tile.TileEffects.WeedProtection))
            ),

            Plant<Plant>("Large plant", "Reduces growth in neighbouring tiles by 50%.", "plant", 8, 0, 8, 5,
                go => NeighbourEffect(go, GrowthInfluence("Large plant adjacency", -.5f))
            ),

            Plant<Plant>("Recyclable plant", "Leaves the tile composted after growing", "plant", 18, 2, 6, 3,
                go => SelfEffect(go, new EffectDefinition() {
                    Do = tiles => { },
                    Undo = Compost(true),
                })
            ),

            Plant<TreePlant>("Small tree", "Doesn't disappear when fully grown.\nProvides shadow in adjacent tiles when grown.", "tree", 20, 3, 0, 2,
                go => NeighbourEffect(go, new EffectDefinition() {
                    Do = tiles => { },
                    Undo = tiles => {
                        foreach (var t in tiles) t.AddEffect(Tile.TileEffects.Shadow);
                    }
                })
            ),

            Plant<Mushrooms>("Mushrooms", "Not affected by growth bonuses, but grows 50% in shadow.", "mushrooms",
                12, 0, 10, 3),
            #endregion

            #region Effect
            Effect("Rain shower", "Accelerates growth by 50%.", 2, GrowthInfluence("Rain shower", .5f), 18, 2),

            Effect("Sprinkler", "Accelerates growth in 3x3 area by 100%.", 4, GrowthInfluence("Sprinkler", 1), 12, 2, 3, 3),

            Effect("Weeds protection", "Protects a 2x2 area from weeds.", 12, TileEffect("Weed protection", Tile.TileEffects.WeedProtection), 15, 1.5f,
                2, 2),
            #endregion

            #region Action
            Action("Bored of it", "Destroy a normal plant.", DestroyProduce<Plant>(), 4, 1, 1, 1, AllProduceOfType<Plant>()),

            Action("Compost", "Compost all empty tiles in a 2x2 area", Compost(), 3, 1.5f, 2, 2, AnyEmpty()),

            Action("Dirty laundry", "Discards entire hand.", t => Hand.Instance.DiscardAll(), 10, .3f),

            Action("Growing spurt", "Advances all procude one extra step.", GrowthStep(1), 5, .5f),

            Action("Lumberjack", "Cuts down a tree", DestroyProduce<TreePlant>(), 8, .8f, 1, 1, AllProduceOfType<TreePlant>()),

            Action("Super effective", "Destroys all weeds", DestroyProduce<Weeds>(), 50, .4f),

            Action("Temporal anomaly", "Instantly finishes a produce.", GrowthStep(1000), 10, .3f, 1, 1, AllProduce()),

            Action("Weeds killer", "Removes weeds from a single tile.", DestroyProduce<Weeds>(), 3, 2.5f, 1, 1, AllProduceOfType<Weeds>()),
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
                obj.transform.localPosition = obj.transform.localPosition = new Vector3(0, 0, -1);;
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
            int cost, float w, int areaW = 0, int areaH = 0, Predicate<Tile[]> areaCheck = null) {
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

    EffectDefinition TileEffect(string desc, Tile.TileEffects effect) {
        return new EffectDefinition() {
            Do = tiles => {
                foreach (var t in tiles)
                    t.AddEffect(effect);
            },
            Undo = tiles => {
                foreach (var t in tiles)
                    t.RemoveEffect(effect);
            }
        };
    }

    Action<Tile[]> Compost(bool filledTilesToo = false) {
        return tiles => {
            foreach (var t in tiles) {
                if (t.plant == null || filledTilesToo)
                    t.Compost();
            }
        };
    }

    Action<Tile[]> GrowthStep(float factor, bool ignoreRate = true) {
        return tiles => {
            foreach (var t in tiles.Where(t1 => t1.plant != null && !(t1.plant is Weeds))) {
                t.DoGrowthStep(factor, ignoreRate);
            }
        };
    }

    Action<Tile[]> DestroyProduce<T>() where T : Plant {
        return tiles => {
            foreach (var t in tiles.Where(t1 => t1.plant != null && (t1.plant is T))) {
                t.BroadcastMessage("DoDestroy");
            }
        };
    }

    Predicate<Tile[]> AllProduce(bool includeWeeds = false) {
        return tiles => tiles.All(t => t.plant != null && (includeWeeds || !(t.plant is Weeds)));
    }
    Predicate<Tile[]> AnyProduce(bool includeWeeds = false) {
        return tiles => tiles.Any(t => t.plant != null && (includeWeeds || !(t.plant is Weeds)));
    }
    Predicate<Tile[]> AllProduceOfType<T>() where T : Plant {
        return tiles => tiles.All(t => t.plant != null && t.plant.GetType() == typeof(T));
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
