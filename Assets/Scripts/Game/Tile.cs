using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviourBase {
    [System.Flags]
    public enum TileEffects {
        None = 0,
        Shadow = 1,
        Composted = 2,
    }

    public enum Selection {
        None,
        Valid,
        Invalid,
    }

    public Tile left, right, top, bottom;
    public float growthRate = 1;
    public Plant plant;
    public TileEffects tileEffects = TileEffects.None;

    public List<GrowthRateInfluence> influences = new List<GrowthRateInfluence>();
    GrowthRateInfluence adjacencyBonus, compostBonus;

    public void DoGrowthStep (float factor = 1, bool ignoreRate = false) {
        BroadcastMessage("GrowthStep", (ignoreRate ? 1 : growthRate) * factor, SendMessageOptions.DontRequireReceiver);
    }

    public void Highlight(Selection selection) {
        switch (selection) {
            case Selection.None:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case Selection.Valid:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case Selection.Invalid:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
    }

    public void AddInfluence(GrowthRateInfluence influence) {
        growthRate += influence.amount;
        influences.Add(influence);
        UpdateInfoMaybe();
    }

    public void EndInfluence(GrowthRateInfluence influence) {
        growthRate -= influence.amount;
        influences.RemoveAll(i => i == influence);
        UpdateInfoMaybe();
    }

    void PlantPlanted(Plant p) {
        plant = p;
        foreach (var t in AdjacentTiles()) {
            t.NeighbourPlantPlanted(p);
            if (t.plant != null && t.plant.title == p.title)
                IncreaseAdjacencyBonus();
        }

        if ((tileEffects & TileEffects.Composted) > 0) {
            if (compostBonus == null) {
                AddInfluence(compostBonus = new GrowthRateInfluence() {
                    description = "Composted",
                    amount = .5f,
                });
            }
            tileEffects -= TileEffects.Composted;
        }

        UpdateInfoMaybe();
    }

    void PlantFinished(Plant p) {
        foreach (var t in AdjacentTiles()) {
            t.NeighbourPlantFinished(p);
        }
        plant = null;

        // End adjacency bonus
        if (adjacencyBonus != null) {
            EndInfluence(adjacencyBonus);
            adjacencyBonus = null;
        }

        // End compost bonus
        if (compostBonus != null) {
            EndInfluence(compostBonus);
            compostBonus = null;
        }
    }

    public void NeighbourPlantPlanted(Plant p) {
        if (plant != null && plant.title == p.title) IncreaseAdjacencyBonus();
    }

    public void NeighbourPlantFinished(Plant p) {
        if (plant != null && plant.title == p.title) DecreaseAdjacencyBonus();
    }

    public void Compost () {
        tileEffects |= TileEffects.Composted;
        UpdateInfoMaybe();
    }

    void IncreaseAdjacencyBonus() {
        if (adjacencyBonus == null) {
            adjacencyBonus = new GrowthRateInfluence() {
                description = "Adjacency bonus"
            };
            influences.Add(adjacencyBonus);
        }

        adjacencyBonus.amount += .1f;
        growthRate += .1f;
    }

    void DecreaseAdjacencyBonus() {
        adjacencyBonus.amount -= .1f;
        growthRate -= .1f;

        if (adjacencyBonus.amount <= 0) {
            adjacencyBonus.amount = 0;
            EndInfluence(adjacencyBonus);
        }
    }

    void UpdateInfoMaybe() {
        if (TileInfo.Instance.selectedTile == this)
            TileInfo.Instance.UpdateInfo();
    }

    public IEnumerable<Tile> AdjacentTiles () {
        if (top != null)
            yield return top;
        if (right != null)
            yield return right;
        if (bottom != null)
            yield return bottom;
        if (left != null)
            yield return left;
    }
}
