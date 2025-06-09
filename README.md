**Descripción corta del repositorio (GitHub “Description”):**

> Magikards: duelo táctico 3D que mezcla estrategia de tablero con cartas mágicas. Defiende, empuja, congela y arrasa: ¡tus hechizos crean espectáculo… y tu equipo historias de terror si no planificas bien!

---

# Magikards

¡Bienvenido a **Magikards**, el juego de estrategia multijugador que combina lo mejor del ajedrez táctico y los juegos de cartas de hechizos! Aquí encontrarás todo lo necesario para que empieces a contribuir en el desarrollo, entender la filosofía de diseño y descubrir cómo levantar tu propio prototipo en Unity.

---

## 📜 Descripción general

En Magikards, cada jugador encarna a un Archimago que dirige un ejército de discípulos (Augures, Arcanistas, Vanguardias) sobre un tablero tridimensional. Con tu mazo de cartas elementales (Fuego, Rayo, Roca, Viento, Hielo) podrás deformar el terreno, invocar criaturas y desatar hechizos que hacen temblar (literalmente) al oponente.
La partida gira en torno a partidas de **15–25 minutos** llenas de decisiones tácticas, combos explosivos y pinceladas de humor mágico.

**Objetivos del repositorio:**

* Servir como base del proyecto: código fuente de Unity, documentación de diseño, assets prototipo.
* Coordinar a los colaboradores: tarea de issues, branch strategies, plantillas de Pull Request.
* Mostrar claramente cómo montar el entorno de desarrollo y los próximos pasos en la hoja de ruta.

---

## 🔮 Características clave

* **Tablero 3D Dinámico**

  * Grid de 13×16 casillas con altura variable.
  * Efectos de terreno según elemento: zonas de lava, pararrayos, cristales helados, corrientes de viento, etc.

* **Fichas con Roles Definidos**

  * **Archimago** (comandante): 7 PV, 2 PA, movilidad 5 casillas.
  * **Augur** (soporte): 5 PV, 1 PA, movimiento tipo “cuadrado” 5×5, curaciones y bendiciones.
  * **Arcanista** (atacante): 5 PV, 2 PA, movilidad diagonal (Alfil) 4 casillas, “Carga Mortal” letal.
  * **Vanguardia** (protector): 7 PV, 1 PA, movimiento ortogonal (Torre) hasta 4 casillas, “Escudo Inquebrantable”.

* **Mecánicas de Cartas**

  * **Fuego**: Alto daño, sobrecalentamiento y daño de retroceso.
  * **Rayo**: Multiimpacto, alto alcance, penalización de aturdimiento.
  * **Roca**: Excepcional defensa y control de terreno; penaliza movilidad propia.
  * **Viento**: Empuja e intercambia fichas; riesgo de mareos y caos posicional.
  * **Hielo**: Congelamientos y ralentizaciones; cuidado con resbalones y sobrecarga de frío.

* **Sistema de Maná y Sobrecarga**

  * Archimagos acumulan hasta 6 PM; el exceso se convierte en fragmentos.
  * Al llegar a 3 fragmentos, entrarás en Sobrecarga: cartas potentes (+efecto), pero con riesgo de incapacitación y daño si no juegas.

* **Multijugador (PvP/PvE)**

  * Prototipo PvP con modo 1 vs 1 (perfil de red en desarrollo).
  * IA básica planeada para enfrentamientos PvE.

---

## 🚀 Instalación y Puesta en Marcha

1. **Clonar el repositorio**

   ```bash
   git clone https://github.com/tu-usuario/Magikards.git
   cd Magikards
   ```

2. **Abrir en Unity**

   * Descargar e instalar Unity (preferiblemente v2022 LTS o posterior).
   * Desde Unity Hub, clic en “Add” → navega hasta la carpeta del proyecto → selecciona “Magikards”.
   * Espera a que Unity importe todos los assets y compile scripts.

3. **Configurar el entorno**

   * Abrir la escena **`Assets/Scenes/Arena_Fuego.unity`** para probar el prototipo de Fuego.
   * Ajustar “Layer Collision Matrix” en **Edit → Project Settings → Physics** si es necesario.
   * Revisar que las “Build Settings” tengan seleccionadas las plataformas necesarias (PC Standalone por defecto).

4. **Ejecutar en el Editor**

   * Con la escena abierta, presiona **Play**.
   * Verás el tablero 13×16 con casillas resaltadas; podrás probar movimientos de ficha y cartas básicas de Fuego.

5. **Dependencias Extras**

   * **DOTween** (Opcional): Para animaciones UI.
   * **Mirror** (en progreso): Networking para PvP.
   * **TextMeshPro**: Librería de texto sofisticado (incluida por defecto en Unity 2022+).

---

## 📖 Uso Básico (Primeros Pasos)

1. **Movimiento de Fichas**

   * Haz clic sobre una ficha (p. ej., Archimago).
   * Verás resaltadas las casillas a las que puede moverse según sus reglas (Reina, Alfil o Torre).
   * Haz clic en la casilla destino para mover la ficha.

2. **Sistema de Turno (placeholder)**

   * Observa en la esquina superior derecha el indicador de “Fase Actual” (Planificación / Ejecución).
   * Aunque la lógica completa de turno está en desarrollo, puedes simular manualmente:

     * **Planificación**: mueve tu ficha.
     * **Fin de turno**: presiona “N” para avanzar de fase (placeholder).

3. **Cartas Básicas de Fuego**

   * Pulsa la tecla **C** para abrir la baraja de prueba (Ignición menor, Bola de fuego, etc.).
   * Selecciona una carta si tienes PM suficientes (ver indicador de Maná).
   * Haz clic en el objetivo para lanzar el hechizo y observa los efectos de quemadura.

---

## 🛠️ Cómo Contribuir

Nos encanta recibir aportes de la comunidad. Si quieres participar, sigue estas pautas:

1. **Fork & Branching**

   * Haz **Fork** del repositorio en tu cuenta.
   * Crea una rama con un nombre descriptivo:

     ```bash
     git checkout -b feature/nombre-de-tu-mejora
     ```
   * Realiza commits pequeños y claros:

     ```bash
     git commit -m "Agrega lógica de movimiento de Augur (sistema 5×5)"
     ```

2. **Pull Requests (PR)**

   * Cuando tu funcionalidad esté lista, empuja tu rama:

     ```bash
     git push origin feature/nombre-de-tu-mejora
     ```
   * Abre un **Pull Request** contra la rama `main`.
   * En la descripción, explica:

     * ¿Qué hace tu cambio?
     * ¿Cómo probarlo?
     * Si resuelve algún issue, menciónalo con `Closes #numero-de-issue`.

3. **Estilo de Código**

   * Usa **C# 8** o superior en Unity.
   * Sigue las convenciones de **naming**:

     * Clases en PascalCase (`MyNewSpell`)
     * Métodos en PascalCase (`CalcularDaño()`)
     * Variables privadas con guion bajo inicial (`_currentMana`).
   * Documenta métodos públicos con comentarios XML breves para IntelliSense.

4. **Testing & Bug Reporting**

   * Si encuentras un bug, crea un **Issue** con:

     * Pasos para reproducir
     * Captura de pantalla o vídeo breve (si aplica)
     * Versión de Unity y plataforma (Windows/macOS/Linux).
   * Si agregas una nueva mecánica, incluye una pequeña “Guía de Pruebas” en `Documentation/Tests/`.

5. **Política de Merges**

   * Todos los PRs se revisarán por al menos dos miembros antes de hacer merge.
   * No se mergea a `main` sin que:

     * Los tests pasen (en desarrollo).
     * La funcionalidad siga el diseño aprobado en el GDD.
     * No introduzca regresiones críticas.

---

## 👥 Equipo Principal

* **Johan Rojas** – Scrum Master, Product Owner, Lead Programmer, Art Leader, Tester
* **Evan Canchica** – Gameplay Programmer, Level Designer, Tester
* **Alexander Casallas** – 3D Artist, 2D Designer, UI/UX, Programmer, Tester
* **Andrés Delgadillo** – Programmer, Tester
* **Jhon Chica** – Programmer, Tester

> **Si quieres unirte al equipo** o colaborar como tester, ¡mira la sección de Contribuciones y crea un issue con tu propuesta!

---

## 🎉 Agradecimientos

* A la comunidad de Discord de Unity por las ideas de optimización de grid.
* A nuestros testers en papel por sobrevivir a las primeras partidas de prototipo (en especial, a “Felipe” que aún no nos habla después del “Tsunami de Rayo”).
* A ti, ¡por revisar este README y ayudarnos a hacer crecer Magikards!

---

¡Nos vemos en la próxima partida (aunque sea en código)!
— **Equipo Magikards**
