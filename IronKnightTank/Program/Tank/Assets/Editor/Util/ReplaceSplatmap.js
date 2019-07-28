import System.IO;
//combine by shenmifangke  
class ReplaceSplatmap extends ScriptableWizard
{
    var Splatmap: Texture2D;
    var New : Texture2D;
    var FlipVertical : boolean;
 
     function OnWizardUpdate(){
         helpString = "Replace the existing splatmap of your terrain with a new one.\nDrag the embedded splatmap texture of your terrain to the 'Splatmap box'.\nThen drag the replacement splatmap texture to the 'New' box\nThen hit 'Replace'.";
         isValid = (Splatmap != null) && (New != null);
     }
	
    function OnWizardCreate () {
   
        if (New.format != TextureFormat.ARGB32 && New.format != TextureFormat.RGB24 && New.format != TextureFormat.RGBA32) {
            EditorUtility.DisplayDialog("Wrong format", "Splatmap must be converted to ARGB 32 bit format.\nMake sure the type is Advanced and set the format!", "Cancel"); 
            return;
        }
	
        var w = New.width;
        if (Mathf.ClosestPowerOfTwo(w) != w) {
            EditorUtility.DisplayDialog("Wrong size", "Splatmap width and height must be a power of two!", "Cancel"); 
            return;	
        }  
 
        try {
            var pixels = New.GetPixels();	
            if (FlipVertical) {
                var h = w; // always square in unity
                for (var y = 0; y < h/2; y++) {
                    var otherY = h - y - 1;	
                    for (var x  = 0; x < w; x++) {
                        var swapval = pixels[y*w + x];					
                        pixels[y*w + x] = pixels[otherY*w + x];
                        pixels[otherY*w + x] = swapval;
                    }		
                }
            }
            Splatmap.Resize (New.width, New.height, New.format, true);
            Splatmap.SetPixels (pixels);
            Splatmap.Apply();
        }
        catch (err) {
            EditorUtility.DisplayDialog("Not readable", "The 'New' splatmap must be readable. Make sure the type is Advanced and enable read/write and try again!", "Cancel"); 
            return;
        }			
    }
    //http://blog.csdn.net/shenmifangke
@MenuItem("Terrain/Replace Splatmap...")
    static function Replace (){
        ScriptableWizard.DisplayWizard(
            "ReplaceSplatmap", ReplaceSplatmap, "Replace");
    }
 
 
@MenuItem("Terrain/Export Texture")
    static function Apply()
    {
        var texture : Texture2D = Selection.activeObject as Texture2D;
        if (texture == null)
        {
            EditorUtility.DisplayDialog("Select Texture", "You Must Select a Texture first!", "Ok");
            return;
        }
 
        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/exported_texture.png", bytes);
    }
}
