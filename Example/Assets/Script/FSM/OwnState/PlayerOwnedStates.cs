using UnityEngine;
namespace PlayerOwnedStates{
    public class Rest : State<Player>{
        public override void Enter(Player entity){
            entity.CurLocation = Locations.Home;
            entity.Fatigue = 0;
            
            entity.PrintText("집에서 쉬기로 해용~");
            entity.PrintText("침대에 누우니 아이고야... 잠이 오네...");
        }

        public override void Execute(Player entity){
            entity.PrintText("드르렁...커어어억...");
            if(entity.Health < 200){
                entity.Health += 10;
            }
            else{
                entity.ChangeState(playerState.Travel);
            }
        }

        public override void Exit(Player entity){
            entity.PrintText("아, 잘 잤다. 자, 이제 나가보기로 해요~");
        }
    }

    public class Travel : State<Player>{
        public override void Enter(Player entity){
            entity.CurLocation = Locations.Dungeon;
            entity.PrintText("자~ 오늘은 던전에서 사냥를! 할거에요.");
        }

        public override void Execute(Player entity){
            entity.PrintText("나는! 나는! 사냥 를 했다!");

            entity.Power ++;
            entity.Health -= 5;
            entity.Fatigue ++;

            //파워가 3이상이 되거나, 10이 된다면...
            if(entity.Power >= 3 && entity.Power <= 10){
                int isExit = Random.Range(0,2);
                if(isExit == 1 || entity.Power == 10){
                    //GoArena 상태로 변경
                    entity.ChangeState(playerState.GoArena);
                }
            }
            
            //체력이 50 이하가 된다면
            if(entity.Health <= 50){
                //TakeADrink 상태로 변경
            }

            //피로가 50을 넘으면
            if(entity.Fatigue >= 50){
                //Rest 상태로 변경
            }  
        }

        public override void Exit(Player entity){
            entity.PrintText("좋았어, 나 에너지 찼다! 다 죽었어!");
        }
    }

    public class GoArena : State<Player>{
        public override void Enter(Player entity){
            entity.CurLocation = Locations.Colosseum;
            entity.PrintText("내 지옥의 역가드 이지선다 를 알까?");
        }
        public override void Execute(Player entity){
            int score = 0;
            if(entity.Power == 10){
                score = 10;
            }
            else{
                int randInd = Random.Range(0,10);
                score = randInd < entity.Power ? Random.Range(6,11) : Random.Range(1,6);
            }

            entity.Power = 0;
            entity.Fatigue += Random.Range(5,11);

            entity.Exp += score;
            entity.PrintText($"10선 를! 하고 {score}만큼 이겼어요. 여태껏 {entity.Exp} 만큼 이겼다 맨이야.");

            if(entity.Exp >= 100){
                Controller.Stop(entity);
                return;
            }
            //점수에 따라서
            //3점 이하면 TakeADrink 상태로 변경
            if(score <= 3){
                entity.PrintText("에이씨 나쁜놈! 진짜 짜증나! 술를! 마실거에요.");
                entity.ChangeState(playerState.TakeADrink);
            }
            //7점 이하면 Travel 상태로 변경
            else if(score <= 7){
                entity.PrintText("자~ 여기까지. 다시 탐험를! 할거에요.");
                entity.ChangeState(playerState.Travel);
            }
            //8점 이상이면 WangOf 상태로 변경
            else if(score >= 8){
                entity.PrintText("봤냐맨이야! 이몸의 실력! 기분 좋게 왕오브를! 할거에요.");
                entity.ChangeState(playerState.WangOf);
            }
        }
        public override void Exit(Player entity){
            entity.PrintText("자 그럼 안녕. 뽕.");
        }
    }
    public class WangOf : State<Player>{
        public override void Enter(Player entity){
            entity.CurLocation = Locations.Studio;
            entity.PrintText("자, 오늘의 상대는 누굴까요 하나 둘 셋 뽈롱~");
        }
        public override void Execute(Player entity){
            entity.PrintText("하하! 불쌍맨 D등급쟁이에요~");

            //확률적으로 
            int randState = Random.Range(0,10);
            //20% 확률로 체력을 잃고 TakeADrink 상태로
            if(randState == 0 || randState == 9){
                entity.Health = entity.Health < 20 ?  0 : entity.Health -= 20;
                entity.PrintText("에이씨! 세컨쟁이였어!");
                entity.PrintText("아이고난!");
                entity.ChangeState(playerState.TakeADrink);
            }
            //80% 확률로 체력 회복 후 Travel 상태로
            else{
                entity.PrintText("하하! 신데구다사이 맞을라구!");
                entity.Health ++;
                entity.Fatigue += 2;

                if(entity.Health >= 100){
                    entity.ChangeState(playerState.Travel);
                }
            }

        }

        public override void Exit(Player entity){
            entity.PrintText("자 수고했어요. 오늘은 여기까지. 뽕.");
        }
    }
    public class TakeADrink : State<Player>{
        public override void Enter(Player entity){
            entity.CurLocation = Locations.Inn;
            entity.PrintText("에이씨, 진탕 마실거야!");
        }
        public override void Execute(Player entity){
            entity.PrintText("아주 그냥 뭉탱이로 마실거야!");
            //체력 회복 + 피로 획득 후
            entity.Health += 5;
            entity.Fatigue += 5;
            //체력을 회복했거나 피로가 너무 높으면 Rest 상태로
            if(entity.Health >= 150 || entity.Fatigue >= 50){
                entity.ChangeState(playerState.Rest);
            }
        }

        public override void Exit(Player entity){
            entity.PrintText("나를 술푸게 하는 살암들...");
        }
    }
}