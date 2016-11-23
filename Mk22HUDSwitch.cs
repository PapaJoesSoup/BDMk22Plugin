using UnityEngine;

namespace BDMk22Plugin
{
    public class Mk22HudSwitch : InternalModule
    {
        private Mk22HUD _hud;
        public bool ButtonHudState;

        public void ButtonHudToggle(bool state)
        {
            if (Mk22HUD.mk22HUDs == null)
                return;

            if (!_hud)
            {
                foreach (var h in Mk22HUD.mk22HUDs)
                    if (h && (h.part == part))
                        _hud = h;

                if (!_hud)
                    return;
            }


            ButtonHudState = state;
            _hud.SetHUD(state);

            Debug.Log("setting state: " + state);
        }

        public bool ButtonHudToggleState()
        {
            Debug.Log("attempting to get hud state");

            if (Mk22HUD.mk22HUDs == null)
            {
                Debug.Log("hud list is null");
                return false;
            }

            if (!_hud)
            {
                foreach (var h in Mk22HUD.mk22HUDs)
                    if (h && (h.part == part))
                        _hud = h;

                if (!_hud)
                {
                    Debug.Log("failed to get hud component");
                    return false;
                }
            }


            Debug.Log("getting state: " + _hud.hasInitialized);

            return _hud.hasInitialized;
        }
    }
}