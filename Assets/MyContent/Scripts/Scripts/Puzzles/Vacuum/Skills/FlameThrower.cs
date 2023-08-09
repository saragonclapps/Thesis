using System.Collections.Generic;

namespace Skills {
    public class FlameThrower : ISkill {
        private IHandEffect _flameVFX;
        private List<IFlamableObjects> _flamableObjectsToInteract;
        private AudioPlayer _audioPlayer;

        public FlameThrower SetFlameVFX(IHandEffect flameVFX) {
            _flameVFX = flameVFX;
            _flameVFX.StopEffect();
            _flameVFX.TerminateEffect();
            return this;
        }
        
        public FlameThrower SetFlamableObjectsToInteract(List<IFlamableObjects> flamableObjectsToInteract) {
            _flamableObjectsToInteract = flamableObjectsToInteract;
            return this;
        }
        
        public FlameThrower SetAudioPlayer(AudioPlayer audioPlayer) {
            _audioPlayer = audioPlayer;
            return this;
        }


        public void Enter() {
            //_flameVFX.StartEffect();
            _audioPlayer.PlayFireAudio();
        }

        public void Execute() {
            if (GameInput.instance.blowUpButton) {
                _flameVFX.StartEffect();
                foreach (var fo in _flamableObjectsToInteract) {
                    fo.SetOnFire();
                }

                SkillManager.instance.RemoveAmountToSkill(0.2f, Skills.FIRE);
            }
            else {
                _flameVFX.StopEffect();
            }
        }

        public void Exit() {
            _flameVFX.StopEffect();
            _flameVFX.TerminateEffect();
            _audioPlayer.StopPowerAudio();
        }
    }
}