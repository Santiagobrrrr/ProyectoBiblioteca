using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Estructuras
{
    public class ArbolBPlus
    {

        // Representa un nodo del árbol, ya sea interno o una hoja.
        private class Nodo
        {
            public bool EsHoja;
            public int CantidadClaves;

            public int[] Claves;
            public Libro?[] Libros;
            public Nodo?[] Hijos;

            // En las hojas apunta a la siguiente hoja para recorrerlas en orden.
            public Nodo? Siguiente;

            public Nodo(int orden, bool esHoja)
            {
                EsHoja = esHoja;
                CantidadClaves = 0;

                // Un nodo de orden m puede guardar hasta m - 1 claves.
                Claves = new int[orden - 1];

                Libros = new Libro?[orden - 1];

                // Un nodo interno puede tener hasta m hijos.
                Hijos = new Nodo?[orden];

                Siguiente = null;
            }
        }

        private Nodo raiz;
        private int orden;

        // Crea el árbol con una hoja vacía como raíz.
        public ArbolBPlus(int orden = 4)
        {
            if (orden < 3)
            {
                orden = 3;
            }

            this.orden = orden;

            // Al inicio la raíz también es una hoja.
            raiz = new Nodo(orden, true);
        }

        // Inserta un libro usando su código como clave.
        public void Insertar(Libro libro)
        {
            // Evita registrar dos libros con el mismo código.
            if (Buscar(libro.Codigo) != null)
            {
                throw new InvalidOperationException(
                    "Ya existe un libro con ese código."
                );
            }

            bool huboDivision = InsertarRecursivo(
                raiz,
                libro,
                out int clavePromovida,
                out Nodo? nuevoDerecho
            );

            // Si la raíz se divide, se crea una nueva raíz.
            if (huboDivision && nuevoDerecho != null)
            {
                Nodo nuevaRaiz = new Nodo(orden, false);

                nuevaRaiz.Claves[0] = clavePromovida;

                nuevaRaiz.Hijos[0] = raiz;
                nuevaRaiz.Hijos[1] = nuevoDerecho;

                nuevaRaiz.CantidadClaves = 1;

                raiz = nuevaRaiz;
            }
        }

        // Inserta de forma recursiva y avisa si un nodo debe dividirse.
        private bool InsertarRecursivo(
            Nodo nodo,
            Libro libro,
            out int clavePromovida,
            out Nodo? nuevoDerecho)
        {
            clavePromovida = 0;
            nuevoDerecho = null;

            // Si llegamos a una hoja, insertamos o dividimos según haya espacio.
            if (nodo.EsHoja)
            {
                if (nodo.CantidadClaves < orden - 1)
                {
                    InsertarEnHoja(nodo, libro);

                    return false;
                }

                DividirHoja(
                    nodo,
                    libro,
                    out clavePromovida,
                    out nuevoDerecho
                );

                return true;
            }

            // Si es un nodo interno, elegimos por cuál hijo continuar.
            int indiceHijo = 0;

            while (
                indiceHijo < nodo.CantidadClaves &&
                libro.Codigo >= nodo.Claves[indiceHijo]
            )
            {
                indiceHijo++;
            }

            Nodo? hijo = nodo.Hijos[indiceHijo];

            if (hijo == null)
            {
                throw new InvalidOperationException(
                    "El Árbol B+ contiene un hijo inválido."
                );
            }

            bool hijoSeDividio = InsertarRecursivo(
                hijo,
                libro,
                out int claveDelHijo,
                out Nodo? derechoDelHijo
            );

            if (!hijoSeDividio || derechoDelHijo == null)
            {
                return false;
            }

            if (nodo.CantidadClaves < orden - 1)
            {
                InsertarEnNodoInterno(
                    nodo,
                    indiceHijo,
                    claveDelHijo,
                    derechoDelHijo
                );

                return false;
            }

            DividirNodoInterno(
                nodo,
                indiceHijo,
                claveDelHijo,
                derechoDelHijo,
                out clavePromovida,
                out nuevoDerecho
            );

            return true;
        }

        // Inserta un libro en una hoja manteniendo las claves ordenadas.
        private void InsertarEnHoja(
            Nodo hoja,
            Libro libro)
        {
            int i = hoja.CantidadClaves - 1;

            while (
                i >= 0 &&
                libro.Codigo < hoja.Claves[i]
            )
            {
                hoja.Claves[i + 1] = hoja.Claves[i];

                hoja.Libros[i + 1] = hoja.Libros[i];

                i--;
            }

            hoja.Claves[i + 1] = libro.Codigo;
            hoja.Libros[i + 1] = libro;

            hoja.CantidadClaves++;
        }

        // Divide una hoja llena y crea una nueva hoja a la derecha.
        private void DividirHoja(
            Nodo hoja,
            Libro libro,
            out int clavePromovida,
            out Nodo? nuevaHoja)
        {
            // Se usa espacio temporal para incluir el nuevo libro antes de dividir.
            int[] clavesTemporales = new int[orden];

            Libro?[] librosTemporales =
                new Libro?[orden];

            int posicion = 0;

            while (
                posicion < hoja.CantidadClaves &&
                hoja.Claves[posicion] < libro.Codigo
            )
            {
                posicion++;
            }

            for (int i = 0; i < posicion; i++)
            {
                clavesTemporales[i] =
                    hoja.Claves[i];

                librosTemporales[i] =
                    hoja.Libros[i];
            }


            clavesTemporales[posicion] =
                libro.Codigo;

            librosTemporales[posicion] =
                libro;

            for (
                int i = posicion;
                i < hoja.CantidadClaves;
                i++)
            {
                clavesTemporales[i + 1] =
                    hoja.Claves[i];

                librosTemporales[i + 1] =
                    hoja.Libros[i];
            }

            int totalClaves =
                hoja.CantidadClaves + 1;

            // Las claves se reparten aproximadamente a la mitad.
            int cantidadIzquierda =
                (totalClaves + 1) / 2;


            hoja.Claves =
                new int[orden - 1];

            hoja.Libros =
                new Libro?[orden - 1];

            hoja.CantidadClaves =
                cantidadIzquierda;

            for (
                int i = 0;
                i < cantidadIzquierda;
                i++)
            {
                hoja.Claves[i] =
                    clavesTemporales[i];

                hoja.Libros[i] =
                    librosTemporales[i];
            }

            nuevaHoja =
                new Nodo(orden, true);

            int cantidadDerecha =
                totalClaves - cantidadIzquierda;

            nuevaHoja.CantidadClaves =
                cantidadDerecha;

            for (
                int i = 0;
                i < cantidadDerecha;
                i++)
            {
                nuevaHoja.Claves[i] =
                    clavesTemporales[
                        cantidadIzquierda + i
                    ];

                nuevaHoja.Libros[i] =
                    librosTemporales[
                        cantidadIzquierda + i
                    ];
            }

            // La nueva hoja se enlaza con la siguiente hoja existente.
            nuevaHoja.Siguiente =
                hoja.Siguiente;

            hoja.Siguiente =
                nuevaHoja;

            // La primera clave de la hoja derecha se usa como separador en el padre.
            clavePromovida =
                nuevaHoja.Claves[0];
        }

        // Inserta una clave promovida y el nuevo hijo derecho.
        private void InsertarEnNodoInterno(
            Nodo nodo,
            int indiceHijo,
            int clave,
            Nodo nuevoDerecho)
        {
            for (
                int i = nodo.CantidadClaves;
                i > indiceHijo;
                i--)
            {
                nodo.Claves[i] =
                    nodo.Claves[i - 1];

                nodo.Hijos[i + 1] =
                    nodo.Hijos[i];
            }

            nodo.Claves[indiceHijo] =
                clave;

            nodo.Hijos[indiceHijo + 1] =
                nuevoDerecho;

            nodo.CantidadClaves++;
        }

        // Divide un nodo interno y promueve su clave central al padre.
        private void DividirNodoInterno(
            Nodo nodo,
            int indiceHijo,
            int nuevaClave,
            Nodo nuevoHijoDerecho,
            out int clavePromovida,
            out Nodo? nuevoDerecho)
        {
            int[] clavesTemporales =
                new int[orden];

            Nodo?[] hijosTemporales =
                new Nodo?[orden + 1];

            for (
                int i = 0;
                i < indiceHijo;
                i++)
            {
                clavesTemporales[i] =
                    nodo.Claves[i];
            }

            clavesTemporales[indiceHijo] =
                nuevaClave;

            for (
                int i = indiceHijo;
                i < nodo.CantidadClaves;
                i++)
            {
                clavesTemporales[i + 1] =
                    nodo.Claves[i];
            }

            for (
                int i = 0;
                i <= indiceHijo;
                i++)
            {
                hijosTemporales[i] =
                    nodo.Hijos[i];
            }

            hijosTemporales[indiceHijo + 1] =
                nuevoHijoDerecho;

            for (
                int i = indiceHijo + 1;
                i <= nodo.CantidadClaves;
                i++)
            {
                hijosTemporales[i + 1] =
                    nodo.Hijos[i];
            }

            int totalClaves =
                nodo.CantidadClaves + 1;


            int medio =
                totalClaves / 2;

            // En un nodo interno, la clave central es la que sube al padre.
            clavePromovida =
                clavesTemporales[medio];

            nodo.Claves =
                new int[orden - 1];

            nodo.Hijos =
                new Nodo?[orden];

            nodo.CantidadClaves =
                medio;

            for (
                int i = 0;
                i < medio;
                i++)
            {
                nodo.Claves[i] =
                    clavesTemporales[i];
            }

            for (
                int i = 0;
                i <= medio;
                i++)
            {
                nodo.Hijos[i] =
                    hijosTemporales[i];
            }

            nuevoDerecho =
                new Nodo(orden, false);


            int cantidadDerecha =
                totalClaves - medio - 1;

            nuevoDerecho.CantidadClaves =
                cantidadDerecha;

            for (
                int i = 0;
                i < cantidadDerecha;
                i++)
            {
                nuevoDerecho.Claves[i] =
                    clavesTemporales[
                        medio + 1 + i
                    ];
            }


            for (
                int i = 0;
                i <= cantidadDerecha;
                i++)
            {
                nuevoDerecho.Hijos[i] =
                    hijosTemporales[
                        medio + 1 + i
                    ];
            }
        }

        // Busca un libro por código hasta llegar a la hoja correspondiente.
        public Libro? Buscar(int codigo)
        {
            Nodo actual = raiz;
            // Baja por los nodos internos hasta llegar a una hoja.
            while (!actual.EsHoja)
            {
                int i = 0;

                while (
                    i < actual.CantidadClaves &&
                    codigo >= actual.Claves[i]
                )
                {
                    i++;
                }


                Nodo? siguiente =
                    actual.Hijos[i];


                if (siguiente == null)
                {
                    return null;
                }


                actual =
                    siguiente;
            }


            for (
                int i = 0;
                i < actual.CantidadClaves;
                i++)
            {
                if (actual.Claves[i] == codigo)
                {
                    return actual.Libros[i];
                }
            }


            return null;
        }

        // Elimina un código y ajusta la raíz si queda vacía.
        public bool Eliminar(int codigo)
        {
            bool eliminado =
                EliminarRecursivo(
                    raiz,
                    codigo
                );

            if (!eliminado)
            {
                return false;
            }

            if (
                !raiz.EsHoja &&
                raiz.CantidadClaves == 0 &&
                raiz.Hijos[0] != null
            )
            {
                raiz =
                    raiz.Hijos[0]!;
            }

            return true;
        }

        // Elimina recursivamente y rebalancea cuando hace falta.
        private bool EliminarRecursivo(
            Nodo nodo,
            int codigo)
        {
            if (nodo.EsHoja)
            {
                return EliminarDeHoja(
                    nodo,
                    codigo
                );
            }

            int indiceHijo = 0;

            while (
                indiceHijo < nodo.CantidadClaves &&
                codigo >= nodo.Claves[indiceHijo]
            )
            {
                indiceHijo++;
            }


            Nodo? hijo =
                nodo.Hijos[indiceHijo];

            if (hijo == null)
            {
                return false;
            }

            bool eliminado =
                EliminarRecursivo(
                    hijo,
                    codigo
                );

            if (!eliminado)
            {
                return false;
            }

            // Si el hijo quedó con pocas claves, se redistribuye o fusiona.
            if (NecesitaRebalanceo(hijo))
            {
                RebalancearHijo(
                    nodo,
                    indiceHijo
                );
            }

            ActualizarSeparadores(nodo);
            return true;
        }

        // Elimina un libro de una hoja y compacta sus elementos.
        private bool EliminarDeHoja(
            Nodo hoja,
            int codigo)
        {
            // Busca la posición del código dentro de la hoja.
            int posicion = -1;

            for (
                int i = 0;
                i < hoja.CantidadClaves;
                i++)
            {
                if (hoja.Claves[i] == codigo)
                {
                    posicion = i;

                    break;
                }
            }

            if (posicion == -1)
            {
                return false;
            }

            for (
                int i = posicion;
                i < hoja.CantidadClaves - 1;
                i++)
            {
                hoja.Claves[i] =
                    hoja.Claves[i + 1];

                hoja.Libros[i] =
                    hoja.Libros[i + 1];
            }

            hoja.CantidadClaves--;

            hoja.Claves[
                hoja.CantidadClaves
            ] = 0;

            hoja.Libros[
                hoja.CantidadClaves
            ] = null;

            return true;
        }

        // Verifica si un nodo quedó por debajo del mínimo permitido.
        private bool NecesitaRebalanceo(
            Nodo nodo)
        {
            if (nodo == raiz)
            {
                return false;
            }

            if (nodo.EsHoja)
            {
                int minimoClavesHoja =
                    orden / 2;

                return
                    nodo.CantidadClaves <
                    minimoClavesHoja;
            }

            int minimoClavesInterno =
                (orden - 1) / 2;

            return
                nodo.CantidadClaves <
                minimoClavesInterno;
        }

        // Rebalancea un hijo según sea hoja o nodo interno.
        private void RebalancearHijo(
            Nodo padre,
            int indiceHijo)
        {
            Nodo? hijo =
                padre.Hijos[indiceHijo];


            if (hijo == null)
            {
                return;
            }

            Nodo? izquierdo = null;
            Nodo? derecho = null;

            if (indiceHijo > 0)
            {
                izquierdo =
                    padre.Hijos[
                        indiceHijo - 1
                    ];
            }


            if (
                indiceHijo <
                padre.CantidadClaves
            )
            {
                derecho =
                    padre.Hijos[
                        indiceHijo + 1
                    ];
            }

            if (hijo.EsHoja)
            {
                RebalancearHoja(
                    padre,
                    indiceHijo,
                    hijo,
                    izquierdo,
                    derecho
                );
            }
            else
            {
                RebalancearNodoInterno(
                    padre,
                    indiceHijo,
                    hijo,
                    izquierdo,
                    derecho
                );
            }

            ActualizarSeparadores(padre);
        }

        // Rebalancea una hoja tomando datos de un hermano o fusionando.
        private void RebalancearHoja(
            Nodo padre,
            int indiceHijo,
            Nodo hoja,
            Nodo? izquierdo,
            Nodo? derecho)
        {
            int minimo =
                orden / 2;

            // Primero se intenta tomar una clave del hermano izquierdo.
            if (
                izquierdo != null &&
                izquierdo.CantidadClaves > minimo
            )
            {
                for (
                    int i = hoja.CantidadClaves;
                    i > 0;
                    i--)
                {
                    hoja.Claves[i] =
                        hoja.Claves[i - 1];

                    hoja.Libros[i] =
                        hoja.Libros[i - 1];
                }


                int ultimaPosicion =
                    izquierdo.CantidadClaves - 1;


                hoja.Claves[0] =
                    izquierdo.Claves[
                        ultimaPosicion
                    ];

                hoja.Libros[0] =
                    izquierdo.Libros[
                        ultimaPosicion
                    ];

                hoja.CantidadClaves++;

                izquierdo.CantidadClaves--;


                izquierdo.Claves[
                    izquierdo.CantidadClaves
                ] = 0;

                izquierdo.Libros[
                    izquierdo.CantidadClaves
                ] = null;


                return;
            }

            // Si no se puede, se intenta tomar una clave del hermano derecho.
            if (
                derecho != null &&
                derecho.CantidadClaves > minimo
            )
            {
                hoja.Claves[
                    hoja.CantidadClaves
                ] = derecho.Claves[0];

                hoja.Libros[
                    hoja.CantidadClaves
                ] = derecho.Libros[0];

                hoja.CantidadClaves++;


                for (
                    int i = 0;
                    i < derecho.CantidadClaves - 1;
                    i++)
                {
                    derecho.Claves[i] =
                        derecho.Claves[i + 1];

                    derecho.Libros[i] =
                        derecho.Libros[i + 1];
                }

                derecho.CantidadClaves--;

                derecho.Claves[
                    derecho.CantidadClaves
                ] = 0;

                derecho.Libros[
                    derecho.CantidadClaves
                ] = null;


                return;
            }

            // Si ningún hermano puede prestar, se fusiona con el izquierdo.
            if (izquierdo != null)
            {
                int posicion =
                    izquierdo.CantidadClaves;

                for (
                    int i = 0;
                    i < hoja.CantidadClaves;
                    i++)
                {
                    izquierdo.Claves[
                        posicion + i
                    ] = hoja.Claves[i];

                    izquierdo.Libros[
                        posicion + i
                    ] = hoja.Libros[i];
                }


                izquierdo.CantidadClaves +=
                    hoja.CantidadClaves;


                izquierdo.Siguiente =
                    hoja.Siguiente;


                EliminarHijoDelPadre(
                    padre,
                    indiceHijo
                );


                return;
            }

            // Si no hay izquierdo disponible, se fusiona con el derecho.
            if (derecho != null)
            {
                int posicion =
                    hoja.CantidadClaves;


                for (
                    int i = 0;
                    i < derecho.CantidadClaves;
                    i++)
                {
                    hoja.Claves[
                        posicion + i
                    ] = derecho.Claves[i];

                    hoja.Libros[
                        posicion + i
                    ] = derecho.Libros[i];
                }


                hoja.CantidadClaves +=
                    derecho.CantidadClaves;


                hoja.Siguiente =
                    derecho.Siguiente;


                EliminarHijoDelPadre(
                    padre,
                    indiceHijo + 1
                );
            }
        }
        // Rebalancea un nodo interno moviendo o fusionando hijos.
        private void RebalancearNodoInterno(
            Nodo padre,
            int indiceHijo,
            Nodo nodo,
            Nodo? izquierdo,
            Nodo? derecho)
        {
            int minimo =
                (orden - 1) / 2;

            // En nodos internos se intenta mover un hijo desde la izquierda.
            if (
                izquierdo != null &&
                izquierdo.CantidadClaves > minimo
            )
            {
                for (
                    int i =
                        nodo.CantidadClaves + 1;
                    i > 0;
                    i--)
                {
                    nodo.Hijos[i] =
                        nodo.Hijos[i - 1];
                }


                nodo.Hijos[0] =
                    izquierdo.Hijos[
                        izquierdo.CantidadClaves
                    ];

                izquierdo.Hijos[
                    izquierdo.CantidadClaves
                ] = null;

                izquierdo.CantidadClaves--;

                nodo.CantidadClaves++;

                ActualizarSeparadores(
                    izquierdo
                );

                ActualizarSeparadores(
                    nodo
                );


                return;
            }

            // Si no es posible, se intenta mover un hijo desde la derecha.
            if (
                derecho != null &&
                derecho.CantidadClaves > minimo
            )
            {
                nodo.Hijos[
                    nodo.CantidadClaves + 1
                ] = derecho.Hijos[0];


                nodo.CantidadClaves++;


                for (
                    int i = 0;
                    i < derecho.CantidadClaves;
                    i++)
                {
                    derecho.Hijos[i] =
                        derecho.Hijos[i + 1];
                }


                derecho.Hijos[
                    derecho.CantidadClaves
                ] = null;


                derecho.CantidadClaves--;


                ActualizarSeparadores(
                    derecho
                );

                ActualizarSeparadores(
                    nodo
                );


                return;
            }

            // Si no se pudo redistribuir, se fusiona con el izquierdo.
            if (izquierdo != null)
            {
                int inicio =
                    izquierdo.CantidadClaves + 1;


                for (
                    int i = 0;
                    i <= nodo.CantidadClaves;
                    i++)
                {
                    izquierdo.Hijos[
                        inicio + i
                    ] = nodo.Hijos[i];
                }


                izquierdo.CantidadClaves =
                    izquierdo.CantidadClaves +
                    nodo.CantidadClaves +
                    1;


                ActualizarSeparadores(
                    izquierdo
                );


                EliminarHijoDelPadre(
                    padre,
                    indiceHijo
                );


                return;
            }

            // Si no hay hermano izquierdo, se fusiona con el derecho.
            if (derecho != null)
            {
                int inicio =
                    nodo.CantidadClaves + 1;


                for (
                    int i = 0;
                    i <= derecho.CantidadClaves;
                    i++)
                {
                    nodo.Hijos[
                        inicio + i
                    ] = derecho.Hijos[i];
                }


                nodo.CantidadClaves =
                    nodo.CantidadClaves +
                    derecho.CantidadClaves +
                    1;


                ActualizarSeparadores(
                    nodo
                );


                EliminarHijoDelPadre(
                    padre,
                    indiceHijo + 1
                );
            }
        }

        // Quita del padre la referencia a un hijo eliminado.
        private void EliminarHijoDelPadre(
            Nodo padre,
            int indiceHijo)
        {
            for (
                int i = indiceHijo;
                i < padre.CantidadClaves;
                i++)
            {
                padre.Hijos[i] =
                    padre.Hijos[i + 1];
            }


            padre.Hijos[
                padre.CantidadClaves
            ] = null;


            padre.CantidadClaves--;


            ActualizarSeparadores(padre);
        }

        // Actualiza las claves que separan los subárboles.
        private void ActualizarSeparadores(
            Nodo nodo)
        {
            if (nodo.EsHoja)
            {
                return;
            }


            for (
                int i = 0;
                i < nodo.CantidadClaves;
                i++)
            {
                Nodo? hijoDerecho =
                    nodo.Hijos[i + 1];


                if (hijoDerecho != null)
                {
                    nodo.Claves[i] =
                        ObtenerPrimeraClave(
                            hijoDerecho
                        );
                }
            }
        }

        // Obtiene la primera clave disponible de un subárbol.
        private int ObtenerPrimeraClave(
            Nodo nodo)
        {
            Nodo actual = nodo;


            while (!actual.EsHoja)
            {
                Nodo? siguiente =
                    actual.Hijos[0];


                if (siguiente == null)
                {
                    throw new InvalidOperationException(
                        "Nodo interno inválido."
                    );
                }

                actual =
                    siguiente;
            }

            if (actual.CantidadClaves == 0)
            {
                throw new InvalidOperationException(
                    "La hoja no contiene claves."
                );
            }

            return actual.Claves[0];
        }

        // Recorre todas las hojas enlazadas desde la más izquierda.
        public void Recorrer()
        {
            Nodo? actual = raiz;


            while (
                actual != null &&
                !actual.EsHoja
            )
            {
                actual =
                    actual.Hijos[0];
            }


            while (actual != null)
            {
                for (
                    int i = 0;
                    i < actual.CantidadClaves;
                    i++)
                {
                    Libro? libro =
                        actual.Libros[i];


                    if (libro != null)
                    {
                        Console.WriteLine(
                            $"{libro.Codigo} - " +
                            $"{libro.Titulo}"
                        );
                    }
                }


                actual =
                    actual.Siguiente;
            }
        }

        // Muestra la estructura completa del Árbol B+.
        public void Imprimir()
        {
            Console.WriteLine(
                "Estructura del Árbol B+:"
            );

            ImprimirNodo(
                raiz,
                0
            );
        }

        // Imprime un nodo y sus hijos de forma recursiva.
        private void ImprimirNodo(
            Nodo nodo,
            int nivel)
        {
            string sangria =
                new string(' ', nivel * 4);


            if (nodo.EsHoja)
            {
                Console.Write(
                    $"{sangria}Hoja: ["
                );
            }
            else
            {
                Console.Write(
                    $"{sangria}Interno: ["
                );
            }


            for (
                int i = 0;
                i < nodo.CantidadClaves;
                i++)
            {
                Console.Write(
                    nodo.Claves[i]
                );


                if (
                    i <
                    nodo.CantidadClaves - 1
                )
                {
                    Console.Write(" | ");
                }
            }

            Console.WriteLine("]");

            if (!nodo.EsHoja)
            {
                for (
                    int i = 0;
                    i <= nodo.CantidadClaves;
                    i++)
                {
                    Nodo? hijo =
                        nodo.Hijos[i];


                    if (hijo != null)
                    {
                        ImprimirNodo(
                            hijo,
                            nivel + 1
                        );
                    }
                }
            }
        }

        // Copia los libros a un arreglo recorriendo las hojas.
        public int CopiarLibros(Libro[] libros)
        {
            Nodo? actual = raiz;
            int posicion = 0;

            while (actual != null && !actual.EsHoja)
            {
                actual = actual.Hijos[0];
            }

            while (actual != null)
            {
                for (int i = 0; i < actual.CantidadClaves; i++)
                {
                    if (actual.Libros[i] != null)
                    {
                        libros[posicion] = actual.Libros[i]!;
                        posicion++;
                    }
                }

                actual = actual.Siguiente;
            }

            return posicion;
        }
    }
}
