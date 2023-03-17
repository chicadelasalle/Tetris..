using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Este scrip es un helper no es necesario relacionarlo con ningun GameObject
public class GridHelper : MonoBehaviour
{
    /*Matriz filas y columnas
     * Ancho y alto de la rejilla
     */
    public static int w = 10, h = 18 + 4;
    //Son estaticas para poder instanciarlas sin tener que consultar el objeto, a su vez estatico es que solo hay uno de ese tipo
    //El +4 es para
    //Creamos el array doble rejilla
    public static Transform[,] grid = new Transform[w, h]; //La como [,] indica dos dimensiones

    //Metodo que dado un vector 2, cogera ese vector y redondeara sus coordenadas de x e y . Tras esto el metodo nos devuelve el vector redondeado
     public static Vector2 RoundVector(Vector2 v)
    {
        //Devuelve un nuevo vector2 ya redondeado en x  y
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));  //Mathf.Round redondea al numero entero mas proximo
    }

    //Metodo que dad una posicion comprobamos si esta pieza esta dentro de los bordes del juego, nos devlvera si es cierto o no
    public static bool IsInsideBorders(Vector2 pos)

    {
        //Si ambas coordenadas son positivas y no se pasan por la derecha
        if(pos.x >= 0 && pos.y >= 0 && pos.x < w)
        {
            //La pieza esta dentro de la zona de juego
            return true;
        }

        //Si lo de arriba no se cumple
        else
        {
            //La pieza esta fuera de la zona de juego
            return false;
        }
    }


    //Un metodo que le pasamos una fila y si hemos comprobado que esta completa la elimina

    public static void DeleteRow(int y)
    {
        //para podes borrar la fila, vemos cada una de las columnas de la fila actual
        for(int x = 0; x < w; x++)
        {
            //Destruyo el cuadrado que hay en esa posicion el objeto qu vensi en la pantalla 
            Destroy(grid[x, y].gameObject);
            //Despues de dstruirlo, el spacio que habi areservado en la rejilla virtual lo vacio
            grid[x, y] = null;

        }

    }

    //Metodo que baja una fila a partir de una fila  concreta

    public static void DecreaseRow(int y)
    {
        //Para poder bajar ña fila, vemos cada una de las columnas de la fila actual
        for (int x = 0; x < w; x++)
        {
            //Si la posicion que quiero bajar no esta vacia 
            if(grid[x, y] != null)
            {
                //Muevo la ficha -1 en la y, a la posicion en la que me encontraba
                grid[x, -1] = grid[x, y];
                //Como hemos bajado el bloque en la poscicion anterior, hacemos null la posicion que ahora ha quedado vacia
                grid[x, y] = null;

                //Ahora repintantamos en pantalla
                //Repintamos en pantalla el bloque una posicion mas abajo de la pantalla por cada bloque
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }



    }

    //Metodo que baja las filas de arriba, a partir e una dada
    public static void DecreaseAbove(int y)
    {
        //Para todas las filas desde la dada
        for(int i = y; i < h; i++)
        {
            //Llamamos al metodo que baja una fila, pero en este caso iran bajando de una en una, hasta que no queden mas
            DecreaseRow(i);
        }
    }

    //Metodo para saber si una fila esta completa, pasandole una fila
    public static bool IsRowFull(int y)
    {
        //Pasaos primero por todas las columnas de esa fila
        for(int x = 0; x < w; x++)
        {
            //Si encuentro algun hueco en esa fila es que no esta llena
            if(grid[x, y] == null)
            {   //Hay un hueco en la fila, no esta completa
                return false;
            }
        }
        //La fila si no se cumple lo de arriba, sera que esta llena
        return true;
    }
  
    //MEtodo para borrar varias o todas las filas de golpe
    public static void DeleteAllFullRows()
    {
        //Comprobamos para odas las filas desde la de mas abajo hasta la de mas arriba
        for(int y = 0; y < h; y++)
            //Si la fila que estamos comprobando esta llena
            if(IsRowFull(y))
            {
                //Borramos la fila actual
                DeleteRow(y);
                //Al borrar la fila actual bajamos las que esten por encima 
                DecreaseAbove(y + 1);
                //Volveriamos a la fila anterior, es decir, si ya hemos borrado una fila, todas bajaran
                //Pero no pasaremos a la siguiente, primero volvemos a comprobar la fila en la que estamos
                y--;
            }
        //Hacemos un borrado de piezas que 
        CleanPieces();
    }

    //Metodo para limpiar piezas cuando ya no tienen bloques
    private static void CleanPieces()
    {
        //Hacemos una pasada por todos los objetos de tipo pieza que encontramos
        foreach(GameObject piece in GameObject.FindGameObjectsWithTag("Piece"))

        {   //Si esa pieza ya no tiene bloques
            if(piece.transform.childCount == 0) //childCount = numero de hijos
            {
                //Destruimos el objeto piece
                Destroy(piece);
            }
        }
    }


}
