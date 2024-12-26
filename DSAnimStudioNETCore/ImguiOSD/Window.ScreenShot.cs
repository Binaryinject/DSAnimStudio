using System.IO;
using System.Windows.Forms;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace DSAnimStudio.ImguiOSD {
    public abstract partial class Window {
        
        public class ScreenShot : Window {
            public override string Title => "Screen Shot";
            public override string ImguiTag => $"{nameof(Window)}.{nameof(ScreenShot)}";
            
            private bool lockAspect = false;
            protected override void BuildContents() {
                if (IsFirstFrameOpen) {
                    GFX.CurrentWorldView.CameraOrbitOriginOffset = Vector3.Zero;
                    GFX.CurrentWorldView.OrbitCamDistanceInput = 5;
                    lockAspect = false;
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
                
                var prefix = WorldView.screenShotPrefix;
                if (ImGui.InputText("前置名称", ref prefix, 256)) {
                    WorldView.screenShotPrefix = prefix;
                }

                var rsw = (int)(Main.TAE_EDITOR.RightSectionWidth - 24);
                if (ImGui.DragInt("分辨率宽度", ref rsw)) {
                    Main.TAE_EDITOR.RightSectionWidth = rsw + 24;
                    if (lockAspect) Main.TAE_EDITOR.TopRightPaneHeight = rsw + 12;
                    Main.RequestViewportRenderTargetResolutionChange = true;
                }
                
                var trh = (int)(Main.TAE_EDITOR.TopRightPaneHeight - 12);
                if (ImGui.DragInt("分辨率高度", ref trh)) {
                    Main.TAE_EDITOR.TopRightPaneHeight = trh + 12;
                    if (lockAspect) Main.TAE_EDITOR.RightSectionWidth = trh + 24;
                    Main.RequestViewportRenderTargetResolutionChange = true;
                }

                if (ImGui.Checkbox("强制比例", ref lockAspect)) {
                    if (lockAspect) {
                        Main.TAE_EDITOR.RightSectionWidth = rsw + 24;
                        Main.TAE_EDITOR.TopRightPaneHeight = rsw + 12;
                        Main.RequestViewportRenderTargetResolutionChange = true;
                    }
                }

                if (ImGui.Button("开始执行步进截图")) {
                    var dialog = new FolderBrowserDialog();
                    if (dialog.ShowDialog() != DialogResult.OK) return;
                    WorldView.screenShotDir = dialog.SelectedPath;
                    Main.TAE_EDITOR.PlaybackCursor.CurrentTime = 0;
                    Main.TAE_EDITOR.Graph.ScrollToPlaybackCursor(1);
                    WorldView.screenShotProcess = true;
                }

            }
        }
    }

}