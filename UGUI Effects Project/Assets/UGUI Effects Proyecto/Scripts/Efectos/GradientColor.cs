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
		#region Metodos
		public override void ModifyMesh(VertexHelper vh)
		{

		}
		#endregion

	}
}
