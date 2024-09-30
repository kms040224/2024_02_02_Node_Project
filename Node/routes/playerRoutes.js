const express = require('express');
const fs = require('fs');       //fs 모듈 추가
const router = express.Router();

//초기 자원 설정
const initialResources =
{
    metal : 500,
    crystal : 300,
    deuterium : 100,

}

//로그인 처리
router.post('/login', (req, res) =>
{

    const {name , password} = req.body;

    if(!global.players[name])
    {
        return res.status(404).send({message: '플레이어를 찾을 수 없습니다.'});
    }

    if(password !== global.players[name.password])
    {
        return res.status(401).send({message:'비밀번호가 틀렸습니다.'});
    }

    //응답 데이터 로그
    const responsePayload =
    {
        playerName: player.playerName,
        metal: player.resources.metal,
        crystal: player.resources.crystal,
        deuterium: player.resources.deuterium
    }

    console.log("Login response payload" , responsePayload); //응답 데이터 로그 추가
    res.send(responsePayload);
    
});
//플레이어 등록 (https://localhost:4000/api/register)
router.post('/register' , (req, res)=>
{
    const {name , password} = req.body;

    if(global.players[name])
    {
        return res.status(400).send({ message : '이미 등록된 사용자이니다.'});
    }

    global.players[name] =
    {
        playerName: name, //player 네임을 설정
        password : password,
        resources:
        {
            metal : 500,
            crystal : 300,
            dueterium : 100
        },
        planets:[]
    };

    saveResources();
    res.send({message : '등록완료' , Player:name});

});

//글로벌 플레이어 객체 초기화
global.players = {};                //글로벌 객체 초기화

module.exports = router;                    //라우터 내보내기