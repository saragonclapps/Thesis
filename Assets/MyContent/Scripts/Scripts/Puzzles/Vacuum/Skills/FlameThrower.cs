using System.Collections.Generic;

namespace Skills {
    public class FlameThrower : ISkill {
        private IHandEffect _flameVFX;
        private List<IFlamableObjects> _flamableObjectsToInteract;
        private AudioPlayerEmitter _audioPlayer;
        private bool _inputBefore;

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
        
        public FlameThrower SetAudioPlayer(AudioPlayerEmitter audioPlayer) {
            _audioPlayer = audioPlayer;
            return this;
        }


        public void Enter() {
            //_flameVFX.StartEffect();
            // _audioPlayer.PlayFireAudio();
        }

        public void Execute() {
            if (!GameInput.instance.blowUpButton) {
                if (_inputBefore) {
                    _inputBefore = false;
                    _audioPlayer.StopPowerAudio();
                }
                _flameVFX.StopEffect();
                return;
            }
            if (!_inputBefore) {
                _audioPlayer.PlayFireAudio();
            }
            _inputBefore = true;
            _flameVFX.StartEffect();
            foreach (var fo in _flamableObjectsToInteract) {
                fo.SetOnFire();
            }

            SkillManager.instance.RemoveAmountToSkill(0.2f, Skills.FIRE);
        }

        public void Exit() {
            // _flameVFX.StopEffect();
            _flameVFX.TerminateEffect();
            _audioPlayer.StopPowerAudio();
        }
    }
}