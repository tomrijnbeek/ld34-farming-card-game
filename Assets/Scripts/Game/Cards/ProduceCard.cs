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
        if (tiles.Length != 1)
            throw new UnityException("Unexpected number of tiles.");

        var obj = Instantiate(prefab);
        obj.transform.parent = tiles[0].transform;
        obj.transform.localPosition = Vector3.zero;
    }
}
