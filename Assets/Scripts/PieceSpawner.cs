using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    //public float speed 10;
    //Array donde guardaremos los objetos que seran spawneados
    public GameObject[] levelPieces;

    //Referencua oara saber la pieza que tenemos u la siguiente
    public GameObject currentPiece, nextPiece;

    // Start is called before the first frame update
    void Start()
    {
        //Sacamos la siguiente pieza para tenerla visible-> Instantiate pieza que debe aparecer, posicion en la que debe aparecer, rotacion con la que debe aparecer
            nextPiece = Instantiate(levelPieces[0], transform.position, Quaternion.identity);
        //nextPiece = Instantiate(levelPiece[0], transform.position, transform.rotation.identity);
        //Activo esa pieza su script para que esa funcione
        SpawnNextPiece();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Metodo para spawnear piezas
    public void SpawnNextPiece()
    {
        //Al ir a spawnear la siguiente pieza, la pieza actual pasa a ser la actual , por lo que activamos 
        currentPiece = nextPiece;
        //Activamos esa pieza (su scrip para que esta funcione)
        currentPiece.GetComponent<Piece>().enabled = true;

           foreach (SpriteRenderer child in gameObject.GetComponentsInChildren<SpriteRenderer>())

        {
            //Cogemos el xolor actual de ese bloque (hijo)
            Color currentColor = child.color;
            //Hacemos algo transparente a ese bloque
            currentColor.a = 1f; //La transparencia va entre 0 y 1, es la A, la propiedad de la transparencia
            child.color = currentColor;
        }

        //LLamamos a la courrutina que prepara la siguiente pieza
        StartCoroutine("PrepareNextPieceCO");

    }

    //Corroutina para preparar 
    IEnumerator PrepareNextPieceCO()
    {
        //Esperamos antes de nada un tiempo
        yield return new WaitForSeconds(0.1f);

        //Tomamos un valor aleatorio comprenido 

        int i = Random.Range(0, levelPieces.Length); //Random.Range(valos mas bajo, y el valor mas alto)
        nextPiece = Instantiate(levelPieces[i], transform.position, Quaternion.identity);
        //Desactivamos el script para que la siguiente pieza no se mueva
        nextPiece.GetComponent<Piece>().enabled = false;
        //Para cada bloque dentro de esa pieza
        
        foreach (SpriteRenderer child in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            //Cogemos el xolor actual de ese bloque (hijo)
            Color currentColor = child.color;
            //Hacemos algo transparente a ese bloque
            currentColor.a = 0.3f; //La transparencia va entre 0 y 1, es la A, la propiedad de la transparencia
            child.color = currentColor;
        }
            
           

    }

}
