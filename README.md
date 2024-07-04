<h1 align="center">CGV 도우미</h1>  


![CGV 도우미](/docs/img/screeen_mainpage.png)  

CGV 도우미는 영화를 좋아하는 사람들을 위한 최고의 도구입니다. 경품 수량 조회, 프로모션 쿠폰 취소표 조회등 공식 앱/웹에서 지원하지 않는 다양한 기능들을 제공하며, 무료로 제공되는 오픈소스 소프트웨어입니다.

웹 프론트엔드는 React,  
백엔드는 ASP.NET Core로 개발되었으며,  
앱은 .NET MAUI Blazor Hybrid 앱으로 개발되었습니다.


<a href=https://cgvmate.com>cgvmate.com</a>에서 웹 버전을 바로 사용할 수 있습니다.  

앱 버전은 현재 개발중에 있습니다.  

<a href="https://api.cgvmate.com/swagger/index.html">api.cgvmate.com</a> 에서 api 명세서를 확인할 수 있습니다. 자유롭게 사용 가능합니다.  

무언가 잘 작동하지 않나요? 혹은 건의사항이 있으신가요?   <a href="https://open.kakao.com/o/sSS6JJsg">카카오 오픈채팅</a>혹은 <a href="https://github.com/woorim02/CGVMate/issues">Github 이슈</a>로 문의해 주세요!

## 지원하는 기능
CGV 도우미는 다음과 같은 기능을 제공합니다.  

 - CGV  
이벤트 조회  
CGV 경품 이벤트 현황 조회, 경품 수량 확인  
스피드쿠폰 취소표 조회  
서프라이즈 쿠폰 취소표 조회  

- 롯데시네마  
이벤트 조회  
롯데시네마 경품 이벤트 현황 조회, 경품 수량 확인  

- 메가박스  
메가박스 경품 이벤트 현황 조회

## 개발중인 기능  
 - CGV  
 영화 오픈 알리미  
 용왕영 경품 자동 신청 기능  

- 롯데시네마  
무비싸다구 취소표 알림 기능

- 3사 공통  
영화 프로모션 알림 기능(앱)  
시사회 신청 알림 기능(앱)  


## 소스코드 사용/프로젝트 기여
이 프로젝트의 소스코드는 GPL-3.0 license에 의해 배포되며, 라이선스에 의거하여 소스코드를 자유롭게 사용할 수 있습니다. 다음과 같은 방식으로 빌드가 가능합니다.  


### CgvMate.Api
    # Publish as docker container

    git clone https://github.com/woorim02/CGVMate.git
    cd CgvMate && cd CgvMate.Api
    dotnet publish --os linux --arch x64 /t:PublishContainer -c Release

docker-compose 예시입니다. 

    # docker-compose.yml
    services:
      cgvmate-api:
        image: 'cgvmate-api:latest'
        restart: always
        container_name: cgvmate-api
        environment:
          - CONNECTION_STRING=${CONNECTION_STRING};
          - IV=${IV}
          - KEY=${KEY}
        ports:
          - 8080:8080

      cgvmate-db:
        image: mariadb:latest  
        restart: always
        container_name: cgvmate-db
        command: --transaction-isolation=READ-COMMITTED --log-bin=binlog --binlog-format=ROW
        environment:
          - MYSQL_ROOT_PASSWORD=${MYSQL_ROOT_PASSWORD}
          - MYSQL_PASSWORD=${MYSQL_PASSWORD}
          - MYSQL_DATABASE=cgvmate
          - MYSQL_USER=cgvmate
        ports:
          - 3306:3306

### CgvMate.Client
    # Publish

    git clone https://github.com/woorim02/CGVMate.git
    cd CgvMate && cd CgvMate.Client
    dotnet publish --configuration Release


## 디버깅
Visual Studio 2022를 사용해 개발하였으며, 디버깅시 다음과 같은 설정이 필요합니다.  

    # 디버깅에 앞서 MariaDb/Mysql 서버를 설정해 주세요
    # 데이터베이스 서버에 맞추어 CgvMate.Api/Program.cs의 CONNECTION_STRING을 적절히 수정해 주세요.

    cd CgvMate.Api
    dotnet ef database update

## 
CGV 도우미 앱은 공식적인 애플리케이션이 아니며, 멀티플렉스 3사와 별도의 연관성이 없습니다. 이 애플리케이션은 3사 영화관 이용자들을 위한 보조 도구로 제공됩니다.