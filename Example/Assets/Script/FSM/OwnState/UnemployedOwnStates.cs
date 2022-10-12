using UnityEngine;

namespace UnemployedOwnStates{
    public class Rest : State<Unemployed>{
        public override void Enter(Unemployed entity){
            entity.CurLocation = Locations.Home;
            entity.Stress = 0;
            entity.Fatigue = 0;

            entity.PrintText("소파에 눕자...");
        }

        public override void Execute(Unemployed entity){
            string state = Random.Range(0,2) == 0 ? "드르렁...커어어억..." : "무한? 무야호~";
            entity.PrintText(state);

            entity.Bored += Random.Range(0,10) < 7 ? 1 : -1;

            if(entity.Bored >= 4){
                entity.ChangeState(UnemployedStates.WangOf);
            }
        }

        public override void Exit(Unemployed entity){
            entity.PrintText("아이고야, 뭐라도 해야지.");
        }
    }

    public class WangOf : State<Unemployed>{
        public override void Enter(Unemployed entity){
            entity.CurLocation = Locations.Studio;
            entity.PrintText("격겜 한판 조져야지~");
        }

        public override void Execute(Unemployed entity){
            int randState = Random.Range(0,10);
            if(randState == 0 || randState == 9){
                entity.PrintText("아니! 겜 하는 꼬라지봐!");
                entity.Stress += 20;
                entity.ChangeState(UnemployedStates.TakeADrink);
            }
            else{
                entity.Bored --;
                entity.Fatigue += 2;

                if(entity.Bored <= 0 || entity.Fatigue >= 50){
                    entity.ChangeState(UnemployedStates.Rest);
                }
            }
        }

        public override void Exit(Unemployed entity){
            entity.PrintText("겜 그만 해야지.");
        }

    }
    
    public class TakeADrink : State<Unemployed>{
        public override void Enter(Unemployed entity){
            entity.CurLocation = Locations.Inn;
            entity.PrintText("오늘도 진탕 마셔야지");
        }

        public override void Execute(Unemployed entity){
            entity.PrintText("캬, 이맛이지!");

            entity.Stress -= 4;
            entity.Fatigue -= 4;

            if(entity.Stress <= 0 || entity.Fatigue >= 50){
                entity.ChangeState(UnemployedStates.Rest);
            }
        }

        public override void Exit(Unemployed entity){
            entity.PrintText("어우, 잘마셨다.");
        }
    }

    public class VisitBathroom : State<Unemployed>{
        public override void Enter(Unemployed entity){
            entity.PrintText("아 배아파.");
        }

        public override void Execute(Unemployed entity){
            entity.PrintText("볼일 좀 본다...");
            entity.RevertToPreviousState();
        }

        public override void Exit(Unemployed entity){
            entity.PrintText("손도 씻고... 나가야지.");
        }
    }

    public class StateGlobal : State<Unemployed>{
        public override void Enter(Unemployed entity){}

        public override void Execute(Unemployed entity){
            if(entity.CurState == UnemployedStates.VisitBathroom){
                return;
            }

            int bathroomState = Random.Range(0,100);
            if(bathroomState < 10){
                entity.ChangeState(UnemployedStates.VisitBathroom);
            }
        }

        public override void Exit(Unemployed entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
