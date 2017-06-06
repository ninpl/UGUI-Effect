//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// BlendColor.cs (06/06/2017)													\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Efecto Blend Color para la UI								\\
// Fecha Mod:		06/06/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#endregion

/*	NOTAS	->
	Blend Color es la tecnica que se usa en la mezcla de colores. Tiene diferentes modos:

	Multiplicar
	Busca la información de color de cada canal y multiplica el color base por el color de fusión. 
	El color resultante siempre es un color más oscuro. Multiplicar cualquier color por negro produce negro. 
	Multiplicar cualquier color por blanco no cambia el color.

	Aditivo
	Este modo de mezcla simplemente agrega valores de píxel de una capa con la otra. 
	En caso de valores superiores a 1 (en el caso de RGB), se visualiza blanco.
	Dado que siempre produce los mismos o más claros colores que la entrada, también se conoce como más ligero. 
	Una variante resta 1 de todos los valores finales, con valores por debajo de 0 convirtiéndose en negro.

	Sustractivo
	Este modo de mezcla simplemente resta los valores de píxel de una capa con la otra. En caso de valores negativos, se muestra negro.

	Sobrecarga
	Este modo de mezcla sobreescribe una capa y otra.
*/

namespace MoonAntonio.UIEffect
{
	/// <summary>
	/// <para>Efecto Blend Color para la UI</para>
	/// </summary>
	[AddComponentMenu("UI/Effects/Blend Color"), RequireComponent(typeof(Graphic))]
	public class BlendColor : BaseMeshEffect
	{
		/// <summary>
		/// <para>Metodo reservado para modificar la malla.</para>
		/// </summary>
		/// <param name="vh">Vertices</param>
		public override void ModifyMesh(VertexHelper vh)// Metodo reservado para modificar la malla
		{
		}
	}
}
