using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviourBase {

    public Tile left, right, top, bottom;
    public float growthRate = 1;
    public Plant plant;

    public List<GrowthRateInfluence> influences = new List<GrowthRateInfluence>();
    GrowthRateInfluence adjacencyBonus;

    public void DoGrowthStep (float factor = 1, bool ignoreRate = false) {
        BroadcastMessage("GrowthStep", (ignoreRate ? 1 : growthRate) * factor, SendMessageOptions.DontRequireReceiver);
    }

    public void Highlight(bool highlight) {
        if (highlight) {
            GetComponent<SpriteRenderer>().color = Color.red;
        } else {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void AddInfluence(GrowthRateInfluence influence) {
        growthRate += influence.amount;
        influences.Add(influence);
    }

    public void EndInfluence(GrowthRateInfluence influence) {
        growthRate -= influence.amount;
        influences.RemoveAll(i => i == influence);
    }

    void PlantPlanted(Plant p) {
        plant = p;
        foreach (var t in AdjacentTiles()) {
            t.NeighbourPlantPlanted(p);
            if (t.plant != null && t.plant.title == p.title)
                IncreaseAdjacencyBonus();
        }
        if (TileInfo.Instance.selectedTile == this)
            TileInfo.Instance.UpdateInfo();
    }

    void PlantFinished(Plant p) {
        foreach (var t in AdjacentTiles()) {
            t.NeighbourPlantFinished(p);
        }
        plant = null;
        if (adjacencyBonus != null) {
            EndInfluence(adjacencyBonus);
            adjacencyBonus = null;
        }
    }

    public void NeighbourPlantPlanted(Plant p) {
        if (plant != null && plant.title == p.title) IncreaseAdjacencyBonus();
    }

    public void NeighbourPlantFinished(Plant p) {
        if (plant != null && plant.title == p.title) DecreaseAdjacencyBonus();
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
