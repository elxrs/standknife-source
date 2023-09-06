using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TMPro.Examples
{
	public class Benchmark03 : MonoBehaviour
	{
		public enum BenchmarkType
		{
			TMP_SDF_MOBILE = 0,
			TMP_SDF__MOBILE_SSD = 1,
			TMP_SDF = 2,
			TMP_BITMAP_MOBILE = 3,
			TEXTMESH_BITMAP = 4
		}

		public int NumberOfSamples = 100;

		public BenchmarkType Benchmark;

		public Font SourceFontFile;

		private void Awake()
		{
		}

		private void Start()
		{
			TMP_FontAsset tMP_FontAsset = null;
			switch (Benchmark)
			{
			case BenchmarkType.TMP_SDF_MOBILE:
				tMP_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256);
				break;
			case BenchmarkType.TMP_SDF__MOBILE_SSD:
				tMP_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256);
				tMP_FontAsset.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field SSD");
				break;
			case BenchmarkType.TMP_SDF:
				tMP_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256);
				tMP_FontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");
				break;
			case BenchmarkType.TMP_BITMAP_MOBILE:
				tMP_FontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SMOOTH, 256, 256);
				break;
			}
			for (int i = 0; i < NumberOfSamples; i++)
			{
				switch (Benchmark)
				{
				case BenchmarkType.TMP_SDF_MOBILE:
				case BenchmarkType.TMP_SDF__MOBILE_SSD:
				case BenchmarkType.TMP_SDF:
				case BenchmarkType.TMP_BITMAP_MOBILE:
				{
					GameObject obj2 = new GameObject();
					obj2.transform.position = new Vector3(0f, 1.2f, 0f);
					TextMeshPro textMeshPro = obj2.AddComponent<TextMeshPro>();
					textMeshPro.font = tMP_FontAsset;
					textMeshPro.fontSize = 128f;
					textMeshPro.text = "@";
					textMeshPro.alignment = TextAlignmentOptions.Center;
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					if (Benchmark == BenchmarkType.TMP_BITMAP_MOBILE)
					{
						textMeshPro.fontSize = 132f;
					}
					break;
				}
				case BenchmarkType.TEXTMESH_BITMAP:
				{
					GameObject obj = new GameObject();
					obj.transform.position = new Vector3(0f, 1.2f, 0f);
					TextMesh textMesh = obj.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = SourceFontFile.material;
					textMesh.font = SourceFontFile;
					textMesh.anchor = TextAnchor.MiddleCenter;
					textMesh.fontSize = 130;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "@";
					break;
				}
				}
			}
		}
	}
}
