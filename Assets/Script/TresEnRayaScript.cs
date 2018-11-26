using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TresEnRayaScript : MonoBehaviour {

    public Camera camara;

    public GameObject prefabFichaPlayer;
    public GameObject prefabFichaRival;

    // TURNOS
    bool turnoPlayer = true;

    // ESTADOS DE QUE TODAS LAS CELDAS ESTAN VACIAS
    int[] celdas = { -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    // CONTADOR TURNOS
    int numTurnos = 1;

    public Text textoGanador;

    public bool hayTresEnRaya = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    /*void Update() {
        // VAMOS A TENER 9 TURNOS
        if (numTurnos > 9) {
            // CADA VEZ QUE PONGO UNA FICHA HAY QUE COMPROBAR SI HAY 3 EN RAYA
            return;
        }

        // SI ES EL TURNO DEL PLAYER Y HE HECHO UNA PULSACION
        // LO QUE SE PONE PRIMERO EN EL IF ES LO MAS FACIL DE EVALUAR Y LUEGO LO MAS
        // COMPLICADO COMO CALCULOS
        if (turnoPlayer && Input.GetMouseButtonDown(0)) {
            //turnoPlayer = false;
            // COGEMOS EL RAYO Y QUE VAYA DESDE LA CAMARA
            // CON LA CAMARA PRINCIPAL POR SI HUBIESE PROBLEMAS
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit rch;
            //bool hasTouch = Physics.Raycast(ray, out rch);
            // Modificamos con respecto al Layer que hemos creado, la capa que haga de filtro
            // rch es la direccion hacia conde quiero que vaya
            if (Physics.Raycast(ray, out rch)) {
                // OBTENEMOS EL INDICE DEL OBJETO QUE ESTAMOS PINCHANDO
                int indice = int.Parse(rch.transform.gameObject.name.Substring(5, 1));
                // ES QUE ESTA RELLENA Y NO PUEDO HPONER MI FICHA
                if (celdas[indice] != -1) {
                    return;
                }
                // SI ESTA VACIA... 

                //print(rch.transform.gameObject.name);
                // INSTANTIATE DE LA FICHA DONDE ESTA EL CUBO
                /*GameObject ficha = Instantiate(prefabFichaPlayer, rch.transform);
                // lo ponemos en el FORWARD porque esta situado las flechas de distinta manera que la de Fernando.
                ficha.transform.Translate(Vector3.forward * 0.1f);


                // INDICE DE CADA UNO DE LOS OBJETOS
                // coge a partir del numero 5 de la cadena un caracter
                //int indice = int.Parse(rch.transform.gameObject.name.Substring(5, 1));
                // LO PONEMOS A FALSE 
                turnoPlayer = false;
                // ESTA EL PLAYER UBICACIOND E LA FICHA EN EL ARRAY
                celdas[indice] = 0;
                // SUMAMOS TURNO
                numTurnos++;


                // LLAMAMOS A COMPROBAR SI HAY TRES EN RAYA
                ComprobarTresEnRaya();

                // SI EL NUMERO DE TURNOS ES MENOS QUE 9 HAREMOS EL INVOKE A TURNOS
                if (numTurnos < 9) {
                    // PARA QUE PAREZCA QUE ESTA PENSANDO
                    Invoke("TurnoRival", 1);
                }

                // METODO NUEVO QUE METEMOS TODO LO ANTERIOR EN UN METODO
                // LO MEJOR SERIA METER VARIOS METODOS PARA PODER USARLOS LO DEJAMOS ASI
                GeneracionFichaYCambioDeTurno(indice,  rch);
            }
            
        }

    }*/


    public void Update() {
        print("UPDATE");
        // VAMOS A TENER 9 TURNOS
        if (hayTresEnRaya == false && numTurnos > 9) {
            print("ENTRA Y RETURN");
            return; // SALE DEL METODO
        }

        // NECESITAMOS LA PULSACION PARA LA TABLET
        if (turnoPlayer &&  
            (Input.touchCount > 0)  && (Input.GetTouch(0).phase == TouchPhase.Began)){
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit rch;

            if (Physics.Raycast(raycast, out rch)) {
                // EL RAYCASTHIT TIENE LA INFORMACION SOBRE EL OBJETO PULSADO
                int indice = int.Parse(rch.transform.gameObject.name.Substring(5, 1));

                // ES QUE ESTA RELLENA Y NO PUEDO HPONER MI FICHA
                if (celdas[indice] != -1) {
                    return;
                }

                GeneracionFichaYCambioDeTurno(rch, indice);
            }

        }
    }

    void GeneracionFichaYCambioDeTurno(RaycastHit rch, int indice) {
        GameObject ficha = Instantiate(prefabFichaPlayer, rch.transform);
        // lo ponemos en el FORWARD porque esta situado las flechas de distinta manera que la de Fernando.
        ficha.transform.Translate(Vector3.forward * 0.1f);


        // INDICE DE CADA UNO DE LOS OBJETOS
        // coge a partir del numero 5 de la cadena un caracter
        //int indice = int.Parse(rch.transform.gameObject.name.Substring(5, 1));
        // LO PONEMOS A FALSE 
        turnoPlayer = false;
        // ESTA EL PLAYER UBICACIOND E LA FICHA EN EL ARRAY
        celdas[indice] = 0;
        // SUMAMOS TURNO
        numTurnos++;


        // LLAMAMOS A COMPROBAR SI HAY TRES EN RAYA
        ComprobarTresEnRaya();

        // SI EL NUMERO DE TURNOS ES MENOS QUE 9 HAREMOS EL INVOKE A TURNOS
        if ( hayTresEnRaya == false && numTurnos < 9 ) {
            // PARA QUE PAREZCA QUE ESTA PENSANDO
            Invoke("TurnoRival", 1);
        }
    }

    void TurnoRival() {
        
        int pos;
        do {
            // GENERO UN ALEATORIO ENTRE 0 Y 8 Y CUANDO ENCUENTRE UNO VACIO SE SALDRA
            // SE REPETIRA HASTA QUE ENCUENTRE UNO VACIO PARA COLOCARSE
            pos = Random.RandomRange(0, 8);
        } while (celdas[pos] != -1);

        /*GameObject ficha = Instantiate(prefabFichaPlayer, rch.transform);
        // lo ponemos en el FORWARD porque esta situado las flechas de distinta manera que la de FErnando.
        ficha.transform.Translate(Vector3.forward * 0.1f);
        */

        // RECOGEMOS EL OBJ CON EL NOMBRE MAS LA POSICION
        GameObject casilla = GameObject.Find("Celda" + pos);
        // INSTANCIAMOS EL PREFAB
        GameObject ficha = Instantiate(prefabFichaRival,  casilla.transform);
        ficha.transform.Translate(Vector3.forward * 0.1f);
        //print(casilla);
        // RELLENAMOS LA POSICION A 1 PARA QUE ESTE RELLENA
        celdas[pos] = 1;
        // PARA QUE TENGA EL TURNO EL PLAYER
        turnoPlayer = true;

        // LLAMAMOS A COMPROBAR SI HAY TRES EN RAYA
        ComprobarTresEnRaya();
    }


    void ComprobarTresEnRaya() {
        hayTresEnRaya = false;
        int ganador = -1;
        // HORIZONTAL
        // AL MENOS UNA DEL ELLAS CELDA TIENE QUE SER DISTINTA DE -1 
        if (celdas[0] != -1 && celdas[0] == celdas[1] && celdas[0] == celdas[2]) {
            hayTresEnRaya = true;
            ganador = celdas[0];
        }
        // HORIZONTAL
        else if (celdas[3] != -1 && celdas[3] == celdas[4] && celdas[3] == celdas[5]) {
            hayTresEnRaya = true;
            ganador = celdas[3];
        } 
        else if (celdas[6] != -1 && celdas[6] == celdas[7] && celdas[6] == celdas[8]) {
            hayTresEnRaya = true;
            ganador = celdas[6];
        }
        else if (celdas[0] != -1 && celdas[0] == celdas[3] && celdas[0] == celdas[6]) {
            hayTresEnRaya = true;
            ganador = celdas[0];
        }
        else if (celdas[1] != -1 && celdas[1] == celdas[4] && celdas[1] == celdas[7]) {
            hayTresEnRaya = true;
            ganador = celdas[1];
        }
        else if (celdas[2] != -1 && celdas[2] == celdas[5] && celdas[2] == celdas[8]) {
            hayTresEnRaya = true;
            ganador = celdas[2];
        }
        // DIAGONAL
        else if (celdas[0] != -1 && celdas[0] == celdas[4] && celdas[0] == celdas[8]) {
            hayTresEnRaya = true;
            ganador = celdas[0];
        // DIAGONAL
        } else if (celdas[2] != -1 && celdas[2] == celdas[4] && celdas[2] == celdas[6]) {
            hayTresEnRaya = true;
            ganador = celdas[2];
        }
        if (hayTresEnRaya) {
            if (ganador == 0) {
                //print("GANA PLAYER");
                textoGanador.text = "GANA JUGADOR";
            } else {
                textoGanador.text = "GANA RIVAL";
            }
            // HABRIA QUE HACER QUE SE PARE CUANDO HA GANADO UNO U OTRO Y NO PONGA MAS PELOTAS.
         

        } else if (numTurnos > 9){
            print("TABLAS");
        }

    }

}
