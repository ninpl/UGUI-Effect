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

	Multiply
	Busca la información de color de cada canal y multiplica el color base por el color de fusión. 
	El color resultante siempre es un color más oscuro. Multiplicar cualquier color por negro produce negro. 
	Multiplicar cualquier color por blanco no cambia el color.

	Additive
	Este modo de mezcla simplemente agrega valores de píxel de una capa con la otra. 
	En caso de valores superiores a 1 (en el caso de RGB), se visualiza blanco.
	Dado que siempre produce los mismos o más claros colores que la entrada, también se conoce como más ligero. 
	Una variante resta 1 de todos los valores finales, con valores por debajo de 0 convirtiéndose en negro.

	Subtractive
	Este modo de mezcla simplemente resta los valores de píxel de una capa con la otra. En caso de valores negativos, se muestra negro.

	Override
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
		#region Variables Privadas
		/// <summary>
		/// <para>Tipo de blend</para>
		/// </summary>
		[SerializeField]
		private BlendModes blendMode = BlendModes.None;										// Tipo de blend
		/// <summary>
		/// <para>Color de blend</para>
		/// </summary>
		[SerializeField]
		private Color color = Color.white;													// Color de blend
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Modo de Blend</para>
		/// </summary>
		public BlendModes BlendMode
		{
			get { return blendMode; }
			set { if (blendMode != value) { blendMode = value; Actualizar(); } }
		}
		/// <summary>
		/// <para>Color de Blend</para>
		/// </summary>
		public Color Color
		{
			get { return color; }
			set { if (color != value) { color = value; Actualizar(); } }
		}
		#endregion

		#region Enum
		/// <summary>
		/// <para>Modos de Blend</para>
		/// </summary>
		public enum BlendModes// Modos de Blend
		{
			None,
			Multiply,
			Additive,
			Subtractive,
			Override,
		}
		#endregion

		#region Metodos
		/// <summary>
		/// <para>Metodo reservado para modificar la malla.</para>
		/// </summary>
		/// <param name="vh">Vertices</param>
		public override void ModifyMesh(VertexHelper vh)// Metodo reservado para modificar la malla
		{
			// Comprobamos si esta activo
			if (IsActive() == false) return;

			// Asignamos las variables
			List<UIVertex> vList = UIEffectListPool<UIVertex>.Get();

			// Obtenemos los vertices
			vh.GetUIVertexStream(vList);

			// Modificamos los vertices
			ModVertices(vList);

			// Limpiamos la lista y agregamos los triangulos
			vh.Clear();
			vh.AddUIVertexTriangleStream(vList);

			// Lanzamos el efecto
			UIEffectListPool<UIVertex>.Release(vList);
		}

		/// <summary>
		/// <para>Modifica los vertices de una lista</para>
		/// </summary>
		/// <param name="vList">Lista de vertices</param>
		private void ModVertices(List<UIVertex> vList)// Modifica los vertices de una lista
		{
			// Comprobamos si esta activo
			if (IsActive() == false || vList == null || vList.Count == 0) return;

			// Asignamos las variables
			UIVertex newVertice;

			// Recorremos la lista
			for (int i = 0; i < vList.Count; i++)
			{
				// Asignamos variables
				newVertice = vList[i];
				byte orgAlpha = newVertice.color.a;

				switch (blendMode)
				{
					case BlendModes.None:
						break;
					case BlendModes.Multiply:
						newVertice.color *= color;
						break;
					case BlendModes.Additive:
						newVertice.color += color;
						break;
					case BlendModes.Subtractive:
						newVertice.color -= color;
						break;
					case BlendModes.Override:
						newVertice.color = color;
						break;
					default:
						break;
				}
				newVertice.color.a = orgAlpha;
				vList[i] = newVertice;
			}
		}

		/// <summary>
		/// <para>Actualizamos los graficos.</para>
		/// </summary>
		private void Actualizar()// Actualizamos los graficos
		{
			if (graphic != null) graphic.SetVerticesDirty();
		}
		#endregion
	}
}
