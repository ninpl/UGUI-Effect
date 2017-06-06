//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// GradientColor.cs (06/06/2017)												\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Efecto Gradient Color para la UI							\\
// Fecha Mod:		06/06/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#endregion

/*	NOTAS	->
	Degradado del color
*/

namespace MoonAntonio.UIEffect
{
	/// <summary>
	/// <para>Efecto Gradient Color para la UI</para>
	/// </summary>
	[AddComponentMenu("UI/Effects/Gradient Color"), RequireComponent(typeof(Graphic))]
	public class GradientColor : BaseMeshEffect
	{
		#region Constantes
		/// <summary>
		/// <para>Vertices de la textura</para>
		/// </summary>
		private const int TextVert = 6;                                     // Vertices de la textura
		#endregion

		#region Variables Privadas
		/// <summary>
		/// <para>Color Top</para>
		/// </summary>
		[SerializeField]
		private Color colorTop = Color.white;											// Color Top
		/// <summary>
		/// <para>Color Bot</para>
		/// </summary>
		[SerializeField]
		private Color colorBottom = Color.white;										// Color Bot
		/// <summary>
		/// <para>Color Left</para>
		/// </summary>
		[SerializeField]
		private Color colorLeft = Color.white;											// Color Left
		/// <summary>
		/// <para>Color Right</para>
		/// </summary>
		[SerializeField]
		private Color colorRight = Color.white;											// Color Right
		/// <summary>
		/// <para>Degradado Vertical</para>
		/// </summary>
		[SerializeField, Range(-1f, 1f)]
		private float gradientOffsetVertical = 0f;										// Degradado Vertical
		/// <summary>
		/// <para>Degradado Horizontal</para>
		/// </summary>
		[SerializeField, Range(-1f, 1f)]
		private float gradientOffsetHorizontal = 0f;									// Degradado Horizontal
		/// <summary>
		/// <para>Separacion de textura</para>
		/// </summary>
		[SerializeField]
		private bool splitTextGradient = false;											// Separacion de textura
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Color Top</para>
		/// </summary>
		public Color ColorTop
		{
			get { return colorTop; }
			set { if (colorTop != value) { colorTop = value; Actualizar(); } }
		}
		/// <summary>
		/// <para>Color Bot</para>
		/// </summary>
		public Color ColorBottom
		{
			get { return colorBottom; }
			set { if (colorBottom != value) { colorBottom = value; Actualizar(); } }
		}
		/// <summary>
		/// <para>Color Left</para>
		/// </summary>
		public Color ColorLeft
		{
			get { return colorLeft; }
			set { if (colorLeft != value) { colorLeft = value; Actualizar(); } }
		}
		/// <summary>
		/// <para>Color Right</para>
		/// </summary>
		public Color ColorRight
		{
			get { return colorRight; }
			set { if (colorRight != value) { colorRight = value; Actualizar(); } }
		}
		/// <summary>
		/// <para>Degradado Vertical</para>
		/// </summary>
		public float GradientOffsetVertical
		{
			get { return gradientOffsetVertical; }
			set { if (gradientOffsetVertical != value) { gradientOffsetVertical = Mathf.Clamp(value, -1f, 1f); Actualizar(); } }
		}
		/// <summary>
		/// <para>Degradado Horizontal</para>
		/// </summary>
		public float GradientOffsetHorizontal
		{
			get { return gradientOffsetHorizontal; }
			set { if (gradientOffsetHorizontal != value) { gradientOffsetHorizontal = Mathf.Clamp(value, -1f, 1f); Actualizar(); } }
		}
		/// <summary>
		/// <para>Separar Textura</para>
		/// </summary>
		public bool SplitTextGradient
		{
			get { return splitTextGradient; }
			set { if (splitTextGradient != value) { splitTextGradient = value; Actualizar(); } }
		}
		#endregion

		#region Metodos
		/// <summary>
		/// <para>Metodo reservado para modificar la malla.</para>
		/// </summary>
		/// <param name="vh">Vertices</param>
		public override void ModifyMesh(VertexHelper vh)// Metodo reservado para modificar la malla.
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
			float minX = 0f, minY = 0f, maxX = 0f, maxY = 0f, width = 0f, height = 0;
			UIVertex newVertex;

			// Recorremos la lista
			for (int i = 0; i < vList.Count; i++)
			{
				if (i == 0 || (splitTextGradient && i % TextVert == 0))
				{
					minX = vList[i].position.x;
					minY = vList[i].position.y;
					maxX = vList[i].position.x;
					maxY = vList[i].position.y;

					int vertNum = splitTextGradient ? i + TextVert : vList.Count;

					for (int k = i; k < vertNum; k++)
					{
						if (k >= vList.Count)
						{
							break;
						}
						UIVertex vertex = vList[k];
						minX = Mathf.Min(minX, vertex.position.x);
						minY = Mathf.Min(minY, vertex.position.y);
						maxX = Mathf.Max(maxX, vertex.position.x);
						maxY = Mathf.Max(maxY, vertex.position.y);
					}

					width = maxX - minX;
					height = maxY - minY;
				}

				// Asignamos las variables
				newVertex = vList[i];
				Color colorOriginal = newVertex.color;
				Color colorVertical = Color.Lerp(colorBottom, colorTop, (height > 0 ? (newVertex.position.y - minY) / height : 0) + gradientOffsetVertical);
				Color colorHorizontal = Color.Lerp(colorLeft, colorRight, (width > 0 ? (newVertex.position.x - minX) / width : 0) + gradientOffsetHorizontal);
				newVertex.color = colorOriginal * colorVertical * colorHorizontal;

				// Agregamos
				vList[i] = newVertex;
			}
		}

		/// <summary>
		/// <para>Actualizamos los graficos</para>
		/// </summary>
		private void Actualizar()// Actualizamos los graficos
		{
			if (graphic != null) graphic.SetVerticesDirty();
		}
		#endregion
	}
}
