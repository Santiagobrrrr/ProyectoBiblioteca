using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Estructuras
{
    public class ArbolBPlus
    {
        // ==========================================
        // NODO INTERNO DEL ÁRBOL B+
        // ==========================================

        private class Nodo
        {
            public bool EsHoja;
            public int CantidadClaves;

            public int[] Claves;
            public Libro?[] Libros;
            public Nodo?[] Hijos;

            // Sirve para enlazar las hojas del B+
            public Nodo? Siguiente;

            public Nodo(int orden, bool esHoja)
            {
                EsHoja = esHoja;
                CantidadClaves = 0;

                // Un árbol de orden m tiene como máximo
                // m - 1 claves por nodo.
                Claves = new int[orden - 1];

                // Los libros se utilizan en las hojas.
                Libros = new Libro?[orden - 1];

                // Un nodo interno puede tener máximo m hijos.
                Hijos = new Nodo?[orden];

                Siguiente = null;
            }
        }


        // ==========================================
        // ATRIBUTOS DEL ÁRBOL
        // ==========================================

        private Nodo raiz;
        private int orden;


        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public ArbolBPlus(int orden = 4)
        {
            // Evitamos órdenes demasiado pequeños.
            if (orden < 3)
            {
                orden = 3;
            }

            this.orden = orden;

            // Al inicio solamente existe una hoja,
            // que también funciona como raíz.
            raiz = new Nodo(orden, true);
        }


        // ==========================================
        // INSERTAR
        // ==========================================

        public void Insertar(Libro libro)
        {
            // No permitimos códigos duplicados.
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

            // Si la raíz fue la que se dividió,
            // debemos crear una nueva raíz.
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


        // ==========================================
        // INSERCIÓN RECURSIVA
        // ==========================================

        private bool InsertarRecursivo(
            Nodo nodo,
            Libro libro,
            out int clavePromovida,
            out Nodo? nuevoDerecho)
        {
            clavePromovida = 0;
            nuevoDerecho = null;


            // ======================================
            // CASO 1: EL NODO ES UNA HOJA
            // ======================================

            if (nodo.EsHoja)
            {
                // Si todavía tiene espacio,
                // simplemente insertamos.
                if (nodo.CantidadClaves < orden - 1)
                {
                    InsertarEnHoja(nodo, libro);

                    return false;
                }

                // Si está llena, la dividimos.
                DividirHoja(
                    nodo,
                    libro,
                    out clavePromovida,
                    out nuevoDerecho
                );

                return true;
            }


            // ======================================
            // CASO 2: NODO INTERNO
            // ======================================

            int indiceHijo = 0;

            // Buscamos por cuál hijo debemos bajar.
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


            // Insertamos recursivamente en el hijo.
            bool hijoSeDividio = InsertarRecursivo(
                hijo,
                libro,
                out int claveDelHijo,
                out Nodo? derechoDelHijo
            );


            // Si el hijo no se dividió,
            // no tenemos que modificar este nodo.
            if (!hijoSeDividio || derechoDelHijo == null)
            {
                return false;
            }


            // Si el nodo padre todavía tiene espacio,
            // insertamos la clave promovida.
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


            // Si el padre también está lleno,
            // debemos dividirlo.
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


        // ==========================================
        // INSERTAR EN UNA HOJA CON ESPACIO
        // ==========================================

        private void InsertarEnHoja(
            Nodo hoja,
            Libro libro)
        {
            int i = hoja.CantidadClaves - 1;

            // Movemos hacia la derecha las claves
            // mayores al nuevo código.
            while (
                i >= 0 &&
                libro.Codigo < hoja.Claves[i]
            )
            {
                hoja.Claves[i + 1] = hoja.Claves[i];

                hoja.Libros[i + 1] = hoja.Libros[i];

                i--;
            }

            // Insertamos en la posición correcta.
            hoja.Claves[i + 1] = libro.Codigo;
            hoja.Libros[i + 1] = libro;

            hoja.CantidadClaves++;
        }


        // ==========================================
        // DIVIDIR UNA HOJA
        // ==========================================

        private void DividirHoja(
            Nodo hoja,
            Libro libro,
            out int clavePromovida,
            out Nodo? nuevaHoja)
        {
            // La hoja está llena.
            // Creamos espacio temporal para incluir
            // también el nuevo libro.
            int[] clavesTemporales = new int[orden];

            Libro?[] librosTemporales =
                new Libro?[orden];


            int posicion = 0;

            // Determinamos dónde debería ir
            // el nuevo código.
            while (
                posicion < hoja.CantidadClaves &&
                hoja.Claves[posicion] < libro.Codigo
            )
            {
                posicion++;
            }


            // Copiamos lo anterior.
            for (int i = 0; i < posicion; i++)
            {
                clavesTemporales[i] =
                    hoja.Claves[i];

                librosTemporales[i] =
                    hoja.Libros[i];
            }


            // Insertamos el nuevo libro.
            clavesTemporales[posicion] =
                libro.Codigo;

            librosTemporales[posicion] =
                libro;


            // Copiamos lo que estaba después.
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


            // Dividimos aproximadamente a la mitad.
            int cantidadIzquierda =
                (totalClaves + 1) / 2;


            // Limpiamos/reconstruimos la hoja original.
            hoja.Claves =
                new int[orden - 1];

            hoja.Libros =
                new Libro?[orden - 1];

            hoja.CantidadClaves =
                cantidadIzquierda;


            // Primera mitad permanece en la hoja vieja.
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


            // Creamos la hoja derecha.
            nuevaHoja =
                new Nodo(orden, true);


            int cantidadDerecha =
                totalClaves - cantidadIzquierda;

            nuevaHoja.CantidadClaves =
                cantidadDerecha;


            // Segunda mitad pasa a la nueva hoja.
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


            // ======================================
            // ENLACE ENTRE HOJAS
            // ======================================

            nuevaHoja.Siguiente =
                hoja.Siguiente;

            hoja.Siguiente =
                nuevaHoja;


            // La primera clave de la hoja derecha
            // sube al padre como separador.
            clavePromovida =
                nuevaHoja.Claves[0];
        }


        // ==========================================
        // INSERTAR EN NODO INTERNO CON ESPACIO
        // ==========================================

        private void InsertarEnNodoInterno(
            Nodo nodo,
            int indiceHijo,
            int clave,
            Nodo nuevoDerecho)
        {
            // Movemos claves e hijos hacia la derecha.
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


        // ==========================================
        // DIVIDIR NODO INTERNO
        // ==========================================

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


            // Copiamos claves anteriores.
            for (
                int i = 0;
                i < indiceHijo;
                i++)
            {
                clavesTemporales[i] =
                    nodo.Claves[i];
            }


            // Insertamos nueva clave.
            clavesTemporales[indiceHijo] =
                nuevaClave;


            // Copiamos claves posteriores.
            for (
                int i = indiceHijo;
                i < nodo.CantidadClaves;
                i++)
            {
                clavesTemporales[i + 1] =
                    nodo.Claves[i];
            }


            // Copiamos hijos anteriores.
            for (
                int i = 0;
                i <= indiceHijo;
                i++)
            {
                hijosTemporales[i] =
                    nodo.Hijos[i];
            }


            // Insertamos nuevo hijo.
            hijosTemporales[indiceHijo + 1] =
                nuevoHijoDerecho;


            // Copiamos los hijos posteriores.
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


            // La clave central sube al padre.
            clavePromovida =
                clavesTemporales[medio];


            // ======================================
            // RECONSTRUIR NODO IZQUIERDO
            // ======================================

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


            // ======================================
            // CREAR NODO DERECHO
            // ======================================

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


        // ==========================================
        // BUSCAR
        // ==========================================

        public Libro? Buscar(int codigo)
        {
            Nodo actual = raiz;


            // Bajamos hasta llegar a una hoja.
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


            // Buscamos dentro de la hoja.
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


        // ==========================================
        // ELIMINAR
        // ==========================================

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


            // Si la raíz interna quedó sin claves,
            // su único hijo pasa a ser la raíz.
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


        // ==========================================
        // ELIMINAR RECURSIVAMENTE
        // ==========================================

        private bool EliminarRecursivo(
            Nodo nodo,
            int codigo)
        {
            // Si llegamos a una hoja,
            // eliminamos directamente.
            if (nodo.EsHoja)
            {
                return EliminarDeHoja(
                    nodo,
                    codigo
                );
            }


            int indiceHijo = 0;


            // Buscamos por cuál hijo bajar.
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


            // Si quedó con menos claves
            // de las permitidas, rebalanceamos.
            if (NecesitaRebalanceo(hijo))
            {
                RebalancearHijo(
                    nodo,
                    indiceHijo
                );
            }


            // Actualizamos separadores internos.
            ActualizarSeparadores(nodo);


            return true;
        }


        // ==========================================
        // ELIMINAR DIRECTAMENTE DE UNA HOJA
        // ==========================================

        private bool EliminarDeHoja(
            Nodo hoja,
            int codigo)
        {
            int posicion = -1;


            // Buscamos el código.
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


            // No existe.
            if (posicion == -1)
            {
                return false;
            }


            // Movemos hacia la izquierda
            // los datos posteriores.
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


            // Limpiamos el espacio sobrante.
            hoja.Claves[
                hoja.CantidadClaves
            ] = 0;

            hoja.Libros[
                hoja.CantidadClaves
            ] = null;


            return true;
        }


        // ==========================================
        // DETERMINAR SI NECESITA REBALANCEO
        // ==========================================

        private bool NecesitaRebalanceo(
            Nodo nodo)
        {
            // La raíz puede tener menos elementos.
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


        // ==========================================
        // REBALANCEAR HIJO
        // ==========================================

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


        // ==========================================
        // REBALANCEAR HOJA
        // ==========================================

        private void RebalancearHoja(
            Nodo padre,
            int indiceHijo,
            Nodo hoja,
            Nodo? izquierdo,
            Nodo? derecho)
        {
            int minimo =
                orden / 2;


            // ======================================
            // PEDIR PRESTADO AL IZQUIERDO
            // ======================================

            if (
                izquierdo != null &&
                izquierdo.CantidadClaves > minimo
            )
            {
                // Hacemos espacio al principio.
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


            // ======================================
            // PEDIR PRESTADO AL DERECHO
            // ======================================

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


                // Movemos el derecho hacia la izquierda.
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


            // ======================================
            // FUSIÓN CON EL IZQUIERDO
            // ======================================

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


                // Enlazamos la lista de hojas.
                izquierdo.Siguiente =
                    hoja.Siguiente;


                EliminarHijoDelPadre(
                    padre,
                    indiceHijo
                );


                return;
            }


            // ======================================
            // FUSIÓN CON EL DERECHO
            // ======================================

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


        // ==========================================
        // REBALANCEAR NODO INTERNO
        // ==========================================

        private void RebalancearNodoInterno(
            Nodo padre,
            int indiceHijo,
            Nodo nodo,
            Nodo? izquierdo,
            Nodo? derecho)
        {
            int minimo =
                (orden - 1) / 2;


            // ======================================
            // PEDIR UN HIJO AL IZQUIERDO
            // ======================================

            if (
                izquierdo != null &&
                izquierdo.CantidadClaves > minimo
            )
            {
                // Abrimos espacio al inicio.
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


            // ======================================
            // PEDIR UN HIJO AL DERECHO
            // ======================================

            if (
                derecho != null &&
                derecho.CantidadClaves > minimo
            )
            {
                nodo.Hijos[
                    nodo.CantidadClaves + 1
                ] = derecho.Hijos[0];


                nodo.CantidadClaves++;


                // Desplazamos hijos del derecho.
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


            // ======================================
            // FUSIÓN CON EL IZQUIERDO
            // ======================================

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


            // ======================================
            // FUSIÓN CON EL DERECHO
            // ======================================

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


        // ==========================================
        // ELIMINAR HIJO DEL PADRE
        // ==========================================

        private void EliminarHijoDelPadre(
            Nodo padre,
            int indiceHijo)
        {
            // Desplazamos los hijos.
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


        // ==========================================
        // ACTUALIZAR CLAVES SEPARADORAS
        // ==========================================

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


        // ==========================================
        // OBTENER PRIMERA CLAVE DE UN SUBÁRBOL
        // ==========================================

        private int ObtenerPrimeraClave(
            Nodo nodo)
        {
            Nodo actual = nodo;


            // Bajamos siempre por el primer hijo
            // hasta llegar a una hoja.
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


        // ==========================================
        // RECORRER ÁRBOL
        // ==========================================

        public void Recorrer()
        {
            Nodo? actual = raiz;


            // Llegamos a la hoja más a la izquierda.
            while (
                actual != null &&
                !actual.EsHoja
            )
            {
                actual =
                    actual.Hijos[0];
            }


            // Gracias al atributo Siguiente
            // podemos recorrer todas las hojas.
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


        // ==========================================
        // IMPRIMIR ESTRUCTURA DEL ÁRBOL
        // ==========================================

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


        // ==========================================
        // IMPRIMIR NODOS RECURSIVAMENTE
        // ==========================================

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


            // Si es nodo interno,
            // imprimimos sus hijos.
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
    }
}