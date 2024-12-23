using System.IO;
using System.Windows.Forms;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace DSAnimStudio.ImguiOSD {
    public abstract partial class Window {
        
        public class ScreenShot : Window {
            public override string Title => "Screen Shot";
            public override string ImguiTag => $"{nameof(Window)}.{nameof(ScreenShot)}";
            protected override void BuildContents() {
                if (IsFirstFrameOpen) {
                    GFX.CurrentWorldView.CameraOrbitOriginOffset = Vector3.Zero;
                    GFX.CurrentWorldView.OrbitCamDistanceInput = 5;
                    GFX.CurrentWorldView.OrbitCamEuler = new Vector3(80, 135, 0);
                }
                var coo = new System.Numerics.Vector3(GFX.CurrentWorldView.CameraOrbitOriginOffset.X,
                    GFX.CurrentWorldView.CameraOrbitOriginOffset.Y, GFX.CurrentWorldView.CameraOrbitOriginOffset.Z);
                if (ImGui.DragFloat3("相机偏移", ref coo)) {
                    GFX.CurrentWorldView.CameraOrbitOriginOffset = coo;
                }

                var ocd = GFX.CurrentWorldView.OrbitCamDistanceInput;
                if (ImGui.DragFloat("相机距离", ref ocd)) {
                    GFX.CurrentWorldView.OrbitCamDistanceInput = ocd;
                }

                var oce = new System.Numerics.Vector3(GFX.CurrentWorldView.OrbitCamEuler.X,
                    GFX.CurrentWorldView.OrbitCamEuler.Y, GFX.CurrentWorldView.OrbitCamEuler.Z);
                if (ImGui.DragFloat3("相机旋转", ref oce)) {
                    GFX.CurrentWorldView.OrbitCamEuler = oce;
                }

                if (ImGui.Button("开始执行步进截图")) {
                    var dialog = new FolderBrowserDialog();
                    if (dialog.ShowDialog() != DialogResult.OK) return;
                    var animateName = Path.GetFileNameWithoutExtension(Main.TAE_EDITOR.SelectedTaeAnim.AnimFileName);
                    WorldView.screenShotDir = $"{dialog.SelectedPath}\\{animateName}";
                    if (!Directory.Exists(WorldView.screenShotDir)) Directory.CreateDirectory(WorldView.screenShotDir);
                    Main.TAE_EDITOR.PlaybackCursor.CurrentTime = 0;
                    Main.TAE_EDITOR.Graph.ScrollToPlaybackCursor(1);
                    WorldView.screenShotProcess = true;
                }

            }
        }
    }

}