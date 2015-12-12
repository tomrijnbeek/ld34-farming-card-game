using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProduceCard : Card {
    public GameObject prefab;
    public Image plantImage;

    private Plant plant;

    void Start () {
        plant = prefab.GetComponent<Plant>();
        plantImage.sprite = plant.sprites[plant.sprites.Length - 1];
    }

    protected override TileSelector CreateTileSelector()
    {
        var selector = gameObject.AddComponent<TileSelector>();
        selector.w = 1;
        selector.h = 1;
        return selector;
    }

    protected override void DoTheThing(Tile[] tiles)
    {
        
    }
}
