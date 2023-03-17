using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    float lastFall = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento  horizontal de la piezas
        //Movimiento de la ficha a la izuierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Llamamos al metodo de movimiento horizpntal y le pasamos la direccion izquierda
            MovePieceHorizontally(-1);
        }
        //Movimiento de la ficha a la derecha
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePieceHorizontally(1);
        }

        //Rotacion de la pieza
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Roto la pieza hacia la derecha
            transform.Rotate(0, 0, -90);
            //Compruebo si la posicion es valida
            if (IsValidPiecePosition())
            {
                //Actualixamos la rejilla guardando la nueva posicion 
                UpdateGrid();
            }
            else
            {
                //Si la posicion no es valida 
            } transform.Rotate(0, 0, 90);

        }

        //Mover la pieza hacia abajo a pulsar la tecla o haya pasado mas de un segundo desde la ultiva ez que se movio

        else if (Input.GetKeyDown(KeyCode.DownArrow) || lastFall >= 1.0f)

        {
            //Muevo la pieza hacia abajo una posicion
            transform.position += new Vector3(0 - 1, 0);
            //Compruebo si la posicion es valida
            if (IsValidPiecePosition())
            {
                //Actualixamos la rejilla guardando la nueva posicion 
                UpdateGrid();
            }

            //Si la posicion no es valida 
            else
            {
                //Revierto el movimieto hacia abajo sumando uno hacia arriba
                transform.position += new Vector3(0, 1, 0);
                //Si ya no pudiese bajar mas con la pieza habria que detectar si es momento de borrar una fila
                GridHelper.DeleteAllFullRows();


                //Hacemos que aparezca una pieza nueva llamando al metodo piece spawner
                FindObjectOfType<PieceSpawner>().SpawnNextPiece(); //Busca un objetp de ese tipo para poder usar sus metodos y variables
                //Desahabilitamos este scrpit para que esta pieza no vurlva a moverse
                this.enabled = false;

            }

            //Reiniciamos el contador de tiempo
            lastFall = 0; // Time.time tiempo actual

        }

        lastFall += Time.deltaTime;
    }
        //Metodo para el movimiento horizontal
        void MovePieceHorizontally(int direction)  //Con direction le pasamos un numero para saber si es a izquierdas o derechas
        {
            //Movemos la pieza en la direccion dada
            transform.position += new Vector3(direction, 0, 0);
            //Comp
            if (IsValidPiecePosition())
            {
                //Actualizamos la rejilla guaradando la nueva posicion en el gridhelper
                UpdateGrid();
            }
            //Si la posicion no es valida
            else
            {
                //Revertimos el movimiento a la posicion en la que estaba antes
                transform.position += new Vector3(-direction, 0, 0);
            }
        }

        //Metodo que comprueba si la posicion en la que se encuentra ahora mismo la pieza es o no valida

        private bool IsValidPiecePosition()

        {
            //Haemos hna pasada por todas las posiciones de los hijos de las piezas, (los bloques)
            foreach (Transform block in this.transform)
            {
                //Recuperamos su posicion(la de los bloques hijos de la pieza) y la redondeamos para que no tenga decimales 
                Vector2 pos = GridHelper.RoundVector(block.position);

                //Si no esta dentro de los bordes, la posicion no es valida. Es decir algunos de os bordes de la pieza se sale de los bordes o esta encima de ellos 
                if (!GridHelper.IsInsideBorders(pos))
                {
                    //Si algun bloque de la pieza no esta en una posicion 
                    return false;
                }

                //Si ya hay otro bloque en eesa misma osicion, la posicion tampoco es valida
                //Coma la posicion podria ser un float(osea tener decimales) la transformamos en numero entero
                Transform possibleObject = GridHelper.grid[(int)pos.x, (int)pos.y];
                //Si ya hay otro objecto y no es hijo del mismo objeto, osea el bloque que hay es de otra pieza

                if (possibleObject != null && possibleObject.parent != this.transform)
                {
                    //La posicion no sera valida
                    return false;
                }
            }
                //Si ninguna cosas de las anteriores se cumple, sera que el bloque o pieza esta en una posicion valida
                return true;
          
        }

    //Metodo que actualiza la rejilla virtual, tras moverse las piezas o bloques a su nueva posicion
    //Lo haremos primero hacienco un borrado de bloques, poniendo primero todo a null, y luego poniendo las posiciones nuevas de esos bloques
    private void UpdateGrid()

    {     //Comparamos si el padere del objeto coincide con el del bloque que estamos mirando 
        for (int y = 0; y < GridHelper.h; y++)

        {
            for (int x = 0; x < GridHelper.w; x++)
            {
                //Comprobaos si en esa posicion no hay un bloque
                if (GridHelper.grid[x, y] != null)
                {
                    //Comprobamos si el padre del bloque es la pieza donde esta este scrip metido
                    if (GridHelper.grid[x, y].parent == this.transform)
                    {
                        //Se carga los bloques que quedan de esa pieza y pone esas posiciones a null
                        GridHelper.grid[x, y] = null;
                    }
                }
            }
            //Insertamos los bloques en las posiciones en las que deben estar
            //Hacemos una pasada por cada uno de los bloques de la pieza actual
            foreach (Transform block in this.transform)
            {
                //Cojo la posicion donde este cada uno de los hijos y la redondeo
                Vector2 pos = GridHelper.RoundVector(block.position);
                //Metemos esa posicion en la posicion de la rejilla virtual que le toque
                GridHelper.grid[(int)pos.x, (int)pos.y] = block;
            }
        }

    }
        
}
