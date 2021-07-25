
$(document).ready(function () {
    if (localStorage['currentUser'] != null) {

        let user = JSON.parse(localStorage['currentUser']);
        let str = "welcome: " + user.FirsName + ' ' + user.LastName
        document.getElementById("namePlace").innerHTML = str;
        if (user.IsManager == false) {
            $("#userManage").hide();
        }
        currentUser = user
        currentUserId = user.UserId
        console.log(currentUserId)

    }
    //global var-------->
    key = "8d1a5ff2a48df63d8f152f0a36a15c7c";
    url = "https://api.themoviedb.org/";
    imagePath = "https://image.tmdb.org/t/p/w500/";


    loadMainPage();
    $("#searchBTN").click(searchSeries);
    $("div").on("click", ".seriesCard", function () { clickSeriesCard(this) });
    $("div").on("click", ".seasonCard", function () { clickSeasonCard(this) });
    $("#nav_logo").click(loadMainPage);

    //live chat fb
    chatMsgArr = [];
    ref = firebase.database().ref("chatMsg");
    listenToNewMessages();

})




//api call for top popular tv show
function loadMainPage() {
    document.getElementById('main2').innerHTML = '';
    let api = "https://api.themoviedb.org/3/tv/popular?api_key=8d1a5ff2a48df63d8f152f0a36a15c7c&language=en-US&page=1";
    ajaxCall("GET", api, "", getPopolarTVSuccessCB, ErrorCB);

}

function openChat() {
    document.getElementById("liveChat").style.display = "block";

}

function closeChat() {
    document.getElementById("liveChat").style.display = "none";

}

//Replacement image if there is an error
function imgError(image) {
    image.onerror = "";
    image.src = "../pages/images/noimage.gif.jpg";
    
    return true;
}

//when user click search btn
function searchSeries() {
    let name = $("#input_search").val();
    let method = "3/search/tv?";
    let moreParams = "&language=en-US&page=1&include_adult=false&";
    let query = "query=" + encodeURIComponent(name);
    api_key = "api_key=" + key;


    let apiCall = url + method + api_key + moreParams + query;
    ajaxCall("GET", apiCall, "", getTVSuccessCB, ErrorCB);
}

//when user click log out btn
function logOut() {
    localStorage.clear();
    window.location.href = "index.html";
}

//api call to get all messages for series
function getMsg(seriesid) {
    let epApi = "../api/Messages?seriesID=" + seriesid + "&currentUser=" + currentUserId;
    ajaxCall("GET", epApi, "", getMsgListSuccessCB, ErrorCB);
}


//when user click on series card
function clickSeriesCard(card) {

    //get series details
    let apiCall = 'https://api.themoviedb.org/3/tv/' + card.id + '?api_key=8d1a5ff2a48df63d8f152f0a36a15c7c&language=en-US';
    ajaxCall("GET", apiCall, "", getSeriesSuccessCB, ErrorCB);

    //get msg for series
    getMsg(card.id);
}

//when user click on season
function clickSeasonCard(card) {
    Stv_id = card.getAttribute('tv-id');
    SseasonNum = card.getAttribute('seasonNum');

    //api call to get all episode for selected season
    let apiCall = 'https://api.themoviedb.org/3/tv/' + Stv_id + '/season/' + SseasonNum + '?api_key=8d1a5ff2a48df63d8f152f0a36a15c7c&language=en-US'
    ajaxCall("GET", apiCall, "", getSeasonSuccessCB, getSeasonErrorCB);

}

//when user click on add episode btn
function addEpisode(btn) {
    currentEPNum = btn.getAttribute('episodeNum');
    let series = {
        SeriesId: currentSeries.id,
        FirsAirDate: currentSeries.first_air_date,
        Name: currentSeries.name.split("'").join('').split('"').join(''),
        OriginCountry: currentSeries.origin_country[0],
        OriginalLanguage: currentSeries.original_language,
        Overview: currentSeries.overview.split("'").join('').split('"').join(''),
        Popularity: currentSeries.popularity,
        PosterPath: currentSeries.poster_path.split("'").join('').split('"').join('')
    }

    //api call to add the series to DB
    let api = "../api/Series";
    ajaxCall("POST", api, JSON.stringify(series), postSeriesSuccessCB, ErrorCB)
}

//render series card from TMDB api result
function renderResult(seriesList) {
    let str = '';

    for (var i = 0; i < seriesList.length; i++) {
        str += '<div id="' + seriesList[i].id + '"class="card seriesCard card-block mx-2" style="width: 18rem;  background-color:rgba(255, 255, 255,0.8);">'
        str += '<img src="' + imagePath + seriesList[i].poster_path + '" class="card-img-top" alt="..." onerror="imgError(this);">'
        str += '<div class="card-body">'
        str += '<h4>' + seriesList[i].name + '</h4>'
        str += '<p class="card-text">first air date:<br>' + seriesList[i].first_air_date + '</p>';
        str += '<h5>reate: ' + seriesList[i].vote_average + '</h5> </div></div>';
    }
    return str;
}

//when user click my series
function loadMySeries() {
    document.getElementById('main2').innerHTML = '';

    let str = "<div class='container'><select class='form-select bg-dark text-white' id='selectSeries' onchange='getEpisodes(this.value)'><option>choose</option></select></div>"
    document.getElementById("main").innerHTML = str;

    //api call the get the user series list
    let epApi = "../api/Series?userId=" + currentUserId;
    ajaxCall("GET", epApi, "", getTvShowListSuccessCB, ErrorCB)
}

//when user click settings
function loadUserSettings() {
    console.log(currentUser)
    document.getElementById('main2').innerHTML = '';
    let str = '<div class="container shadow p-3 mb-5 bg-white rounded " style="height: 800px;">';
    str += ' <form id="userSettingsForm" action="">';
    str += ' <h2>Personal details </h2>';
    str += '  <div class="form-group">';
    str += ' <label for="firsNameTB">First name</label>';
    str += ' <input type="text" class="form-control" id="firsNameTB" style=" font-size:2rem;" placeholder="' + currentUser.FirsName + '" ></div>';
    str += ' <div class="form-group"><label for="lastNameTB">Last name</label>';
    str += ' <input type="text" class="form-control" id="lastNameTB" style=" font-size:2rem;" placeholder="' + currentUser.LastName + '" > </div>';
    str += '<div class="form-group"><label for="favCategory">Choose a favorite category:  .</label>';
    str += '<select id="favCategory" name="favCategory" class="btn btn-outline-secondary"><option value="0">category</option>';
    str += '<option value="Action">Action</option><option value="Comedy">Comedy</option>';
    str += '<option value="Drama">Drama</option> <option value="Fantasy">Fantasy</option>';
    str += '<option value="Horror">Horror</option><option value="Mystery">Mystery</option>';
    str += '<option value="Romance">Romance</option><option value="Thriller">Thriller</option>';
    str += ' </select> </div> <div class="form-group">';
    str += '<label for="adrressTB">Adrress</label>';
    str += '<input type="text" class="form-control" id="adrressTB" style=" font-size:2rem;" placeholder="' + currentUser.Address + '" >  </div>';
    str += '<input type="submit" value="save changes" class="btn btn-lg btn-light btn-outline-primary" /> </form></div>';

    document.getElementById('main').innerHTML = str;

    $("#userSettingsForm").submit(changeUserInfo)

}

function changeUserInfo() {
    let User = {
        FirsName: $("#firsNameTB").val(),
        LastName: $("#lastNameTB").val(),
        FavCategory: $("#favCategory").val(),
        Address: $("#adrressTB").val(),
        Email: currentUser.Email,
        UserId: currentUser.UserId
    }

    let api = "../api/User"
    ajaxCall("PUT", api, JSON.stringify(User), UpdateUserSuccessCB, ErrorCB);

    return false;
}

function UpdateUserSuccessCB(user) {
    console.log(user)
    localStorage['currentUser'] = JSON.stringify(user);
    document.location.reload(true);
}

//when user click Managment(only for manager user)
function loadManagement() {
    document.getElementById('main2').innerHTML = '';
    //render the tables
    //users information table
    let str = '<section class="page-section  mb-0" id="main"> <div class="container">';
    str += '<h2 class="page-section-heading text-uppercase text-white">users information</h2>';
    str += '<div class="row rounded" id="usersTableDiv" style="background-color: white;">';
    str += '<table id="usersTable" class="display" style="width:100%;"><thead>';
    str += '<tr><th>user id</th>';
    str += '<th>firs name</th>';
    str += '<th>last name</th>';
    str += '<th>email</th>';
    str += '<th>phone number</th>';
    str += '<th>gender</th>';
    str += '<th>birth year</th>';
    str += '<th>favorit category</th>';
    str += '<th>adress</th></tr></thead></table></div>';
    str += '</div></section>';

    //Episodes information table
    let str2 = '<section class="page-section  mb-0" id="main"> <div class="container">';
    str2 += '<h2 class="page-section-heading text-uppercase text-white">Episodes information</h2>';
    str2 += '<div class="row rounded" id="EpisodeTableDiv" style="background-color: white;">';
    str2 += '<table id="EpisodesTable" class="display" style="width:100%;"><thead>';
    str2 += '<tr><th>series id</th>';
    str2 += '<th>episode id </th>';
    str2 += '<th>episode name</th>';
    str2 += '<th>season number</th>';
    str2 += '<th>imgURL</th>';
    str2 += '<th>overview</th>';
    str2 += '<th>number of users</th>';
    str2 += '</tr></thead></table></div>';
    str2 += '</div></section>';

    //Series information table
    let str3 = '<section class="page-section  mb-0" id="main"> <div class="container">';
    str3 += '<h2 class="page-section-heading text-uppercase text-white">Series information</h2>';
    str3 += '<div class="row rounded" id="SeriesTableDiv" style="background-color: white;">';
    str3 += '<table id="SeriesTable" class="display" style="width:100%;"><thead>';
    str3 += '<tr><th>series id</th>';
    str3 += '<th>firt air date</th>';
    str3 += '<th>name</th>';
    str3 += '<th>origin country</th>';
    str3 += '<th>original language</th>';
    str3 += '<th>overview</th>';
    str3 += '<th>popularity</th>';
    str3 += '<th>posterPath</th>';
    str3 += '<th>number of users</th>';
    str3 += '</tr></thead></table></div>';
    str3 += '</div></section>';

    document.getElementById("main").innerHTML = str;
    document.getElementById("main").innerHTML += str2;
    document.getElementById("main").innerHTML += str3;

    //api call for the tables information
    let epApi = "../api/User?managerId=" + currentUserId;
    ajaxCall("GET", epApi, "", getUsersExaptMsuccessCB, ErrorCB);

    let api = "../api/Episodes";
    ajaxCall("GET", api, "", getAllEpisodesSuccessCB, ErrorCB);

    let api2 = "../api/Series";
    ajaxCall("GET", api2, "", getAllSeriesSuccessCB, ErrorCB);
}


//when user click on like btn
function doLike(btn) {
    $("#" + btn.id).css("visibility", "hidden");
    let dislikeBTN = btn.getAttribute("dislikeid")
    $("#" + dislikeBTN).css("visibility", "visible");

    let msgId = btn.getAttribute("msg-id");

    let api = "../api/Messages/DoLike/" + msgId + "/" + currentUserId;
    ajaxCall("POST", api, "", MsgSuccessCB, ErrorCB);
}

//when user click on dislick btn
function doDislike(btn) {
    $("#" + btn.id).css("visibility", "hidden");
    let likeBTN = btn.getAttribute("likeid")
    $("#" + likeBTN).css("visibility", "visible");

    let msgId = btn.getAttribute("msg-id");

    let api = "../api/Messages/DoDislike/" + msgId + "/" + currentUserId;
    ajaxCall("DELETE", api, "", MsgSuccessCB, ErrorCB);

}

//when user click add comment(massege)
function addComment() {
    let msg = {
        Text: $("#adCommentPlace").val(),
        UserIdMsg: currentUserId,
        SeriesId: currentSeries.id
    }
    $("#adCommentPlace").val('');



    let api = "../api/Messages/";
    ajaxCall("POST", api, JSON.stringify(msg), MsgSuccessCB, ErrorCB);

}

function postSeriesForCommentSuccessCB() {

}


//when user click add comment in comment
function addCommentToComment(btn) {
    let text = $("#input_comment_" + btn.id).val();

    perentMsgId = btn.getAttribute("to-msg")


    let msg = {
        Text: text,
        UserIdMsg: currentUserId,
        SeriesId: currentSeries.id
    }
    $("#input_comment").val('');

    let api = "../api/Messages?perendMsgID=" + perentMsgId;
    ajaxCall("PUT", api, JSON.stringify(msg), MsgSuccessCB, ErrorCB);
    console.log(msg);

}

//when user click myFavActors
function loadMyActorss() {
    let epApi = "../api/Actors?userId=" + currentUserId;
    ajaxCall("GET", epApi, "", getActorsDBSuccessCB, ErrorCB)
}

//get all the episode that user save in series
function getEpisodes(seriesId) {
    let epApi = "../api/Episodes?userId=" + currentUserId + "&seriesId=" + seriesId;
    ajaxCall("GET", epApi, "", getEpisodesSuccessCB, ErrorCB)
}

function removeEpisode(Removebtn) {
    let episodeId = Removebtn.getAttribute('episode-id');
    let seriesId = Removebtn.getAttribute('serID');

    let epApi = "../api/Episodes/DeletefromUserLikeEpisode/" + currentUserId + "/" + episodeId;
    ajaxCall("DELETE", epApi, "", function () { deleteEpisodesSuccessCB(seriesId) }, ErrorCB)
}


function getPopolarTVSuccessCB(seriesList) {
    //render top popular tv show to main page
    let str = '<section class="page-section  mb-0" > <div class="container">';
    str += '<h2 class="page-section-heading text-uppercase text-white">Popular now</h2>'
    str += '<div class="row card_row rounded d-flex flex-row flex-nowrap overflow-auto shadow ">'


    str += renderResult(seriesList.results);

    str += '</div> </div></section>';

    document.getElementById("main").innerHTML = str;

    //api call for top rated tv show
    let api = 'https://api.themoviedb.org/3/tv/top_rated?api_key=8d1a5ff2a48df63d8f152f0a36a15c7c&language=en-US&page=1';
    ajaxCall("GET", api, "", getBestRateTVSuccessCB, ErrorCB);

}

function getBestRateTVSuccessCB(seriesList) {
    //render all top rated tv show
    let str = '<section class="page-section  mb-0" > <div class="container">';
    str += '<h2 class="page-section-heading text-uppercase text-white">Best rate</h2>'
    str += '<div class="row card_row rounded d-flex flex-row flex-nowrap overflow-auto shadow ">'

    str += renderResult(seriesList.results);

    str += '</div> </div></section>';

    document.getElementById("main").innerHTML += str;

    //api call for recommend tv show for user(Based on other users with common tv show)
    let api = '../api/Series/getRecommendSeriesForUser/' + currentUserId;
    ajaxCall("GET", api, "", getRecommendSeriesTVSuccessCB, ErrorCB);
}

function getRecommendSeriesTVSuccessCB(seriesList) {

    //render recomended tv show for user
    let str = '<section class="page-section  mb-0" > <div class="container">';
    str += '<h2 class="page-section-heading text-uppercase text-white">Recommended for you</h2>'
    str += '<div class="row card_row rounded d-flex flex-row flex-nowrap overflow-auto shadow ">'

    for (var i = 0; i < seriesList.length; i++) {
        str += '<div id="' + seriesList[i].SeriesId + '"class="card seriesCard card-block mx-2" style="width: 18rem;  background-color:rgba(255, 255, 255,0.8);">'
        str += '<img src="' + imagePath + seriesList[i].PosterPath + '" class="card-img-top" alt="..." onerror="imgError(this);">'
        str += '<div class="card-body">'
        str += '<h4>' + seriesList[i].Name + '</h4>'
        str += '<p class="card-text">first air date:<br>' + seriesList[i].FirsAirDate + '</p>';
        str += '</div></div>';
    }

    str += '</div> </div></section>';

    document.getElementById("main").innerHTML += str;
}


function getTVSuccessCB(seriesList) {

    //render result for search
    let str = '<section class="page-section  mb-0" id="main"> <div class="container">';
    str += '<h2 class="page-section-heading text-uppercase text-white">Results for: "' + $("#input_search").val() + '"</h2>'
    str += '<div class="row card_row rounded d-flex flex-row flex-nowrap overflow-auto shadow ">'

    str += renderResult(seriesList.results);

    str += '</div> </div>  </section>';

    document.getElementById("main").innerHTML = str;
    $("#input_search").val('');
}

function getSeriesSuccessCB(series) {
    //get actors list api
    let apiCall = 'https://api.themoviedb.org/3/tv/' + series.id + '/credits?api_key=8d1a5ff2a48df63d8f152f0a36a15c7c&language=en-US';
    ajaxCall("GET", apiCall, "", getActorsSuccessCB, ErrorCB);

    currentSeries = series;
    //render series info and season list
    let str = ' <div class="container seriesDiv rounded " >';
    str += '<div class="row seriesInfo">';
    str += ' <div class="left col-4">';
    str += ' <img class="seriesMainImg rounded" src="' + imagePath + series.poster_path + '" onerror="imgError(this);"/></div>';
    str += '   <div class="right col-8 text-white">';
    str += '   <h1 class="seriesHeader text-uppercase">' + series.name + '</h1>';
    str += '   <p class="overviewP">' + series.overview + '</p>';
    str += '     <h4>Rate: ' + series.vote_average + '</h4><h4>Language: ' + series.original_language + '</h4><h4>First air date: ' + series.first_air_date + '</h4></div></div>';
    str += '<h2 class="page-section-heading text-uppercase text-white">seasons</h2>';

    str += '<div class="row card_row rounded d-flex flex-row flex-nowrap overflow-auto shadow " >'

    for (var i = 0; i < series.seasons.length; i++) {

        str += '<div tv-id="' + series.id + '" seasonNum="' + i + '" class="card seasonCard card-block mx-2" style="width: 18rem;  background-color:rgba(255, 255, 255,0.8);">'
        str += '<img src="' + imagePath + series.seasons[i].poster_path + '" class="card-img-top" alt="..." onerror="imgError(this);">'
        str += '<div class="card-body">'
        str += '<h4>' + i + ': ' + series.seasons[i].name + '</h4>'
        str += '<p class="card-text">Episodes number:<br>' + series.seasons[i].episode_count + '</p >';
        str += ' </div> </div>';
    }
    str += '</div></div>';
    document.getElementById("main").innerHTML = str;

}

function getSeasonSuccessCB(season) {

    currentSeason = season;
    //render all episode for selected season
    let str = ' <div class="container rounded ">';
    for (var i = 0; i < season.episodes.length; i++) {

        str += '<div class="row episodeDiv rounded text-white">';
        str += ' <div class="left col-3">';
        str += ' <img class="episode_img rounded" src="' + imagePath + season.episodes[i].still_path + '" onerror="imgError(this);"/></div>';
        str += ' <div class="right col-9">';
        str += ' <h2 class="text-uppercase">' + (i + 1) + ': ' + season.episodes[i].name + '</h2>';
        str += ' <p class="overviewP">' + season.episodes[i].overview + '</p>';
        str += ' <h4>Rate:' + season.episodes[i].vote_average + '</h4>';
        str += '<button episodeNum="' + i + '" class="btn btn-lg btn-outline-info" onclick="addEpisode(this)">Add to my list</button></div></div>';

    }
    str += '</div>'

    document.getElementById("main").innerHTML = str;

}

function getTvShowListSuccessCB(seriesList) {
    //render series name to select
    let str = '';
    for (var i = 0; i < seriesList.length; i++) {
        str += '<option value="' + seriesList[i].SeriesId + '">' + seriesList[i].Name + '</option>';
    }
    document.getElementById("selectSeries").innerHTML += str;
}

function getEpisodesSuccessCB(epList) {

    //render episodes list
    let str = ' <div class="container rounded ">';
    for (var i = 0; i < epList.length; i++) {

        str += '<div class="row episodeDiv rounded text-white">';
        str += ' <div class="left col-3">';
        str += ' <img class="episode_img rounded" src="' + epList[i].ImgURL + '" onerror="imgError(this);"/></div>';
        str += ' <div class="right col-9">';
        str += ' <h2 class="text-uppercase">' + (i + 1) + ': ' + epList[i].EpisodeName + '</h2>';
        str += ' <p class="overviewP">' + epList[i].Description + '</p>';
        str += '<h3>Seasone number: ' + epList[i].SeasonNum + '</h3>';
        str += '<button class="btn btn-danger" episode-id="' + epList[i].EpisodeID + '" serID="' + epList[i].SeriesID + '" onclick="removeEpisode(this)">remove</button>';
        str += '</div></div>';

    }
    str += '</div>'

    document.getElementById("main2").innerHTML = str;
}

//render the data to the user managment table in dataTable format
function getUsersExaptMsuccessCB(usersList) {
    try {
        tbl = $('#usersTable').DataTable({
            data: usersList,
            pageLength: 5,
            columns: [
                { data: "UserId" },
                { data: "FirsName" },
                { data: "LastName" },
                { data: "Email" },
                { data: "PhoneNumber" },
                { data: "Gender" },
                { data: "BirthYear" },
                { data: "FavCategory" },
                { data: "Address" },
            ],
        });

    }
    catch (err) {
        alert(err);
    }
}

//render the data to the episode managment table in dataTable format
function getAllEpisodesSuccessCB(epList) {
    try {
        tbl = $('#EpisodesTable').DataTable({
            data: epList,
            pageLength: 5,
            columns: [
                { data: "SeriesID" },
                { data: "EpisodeID" },
                { data: "EpisodeName" },
                { data: "SeasonNum" },
                { data: "ImgURL" },
                { data: "Description" },
                { data: "NumOfUsers" },

            ],
        });

    }
    catch (err) {
        alert(err);
    }
}

//render the data to the series managment table in dataTable format
function getAllSeriesSuccessCB(seriesList) {
    try {
        tbl = $('#SeriesTable').DataTable({
            data: seriesList,
            pageLength: 5,
            columns: [
                { data: "SeriesId" },
                { data: "FirsAirDate" },
                { data: "Name" },
                { data: "OriginCountry" },
                { data: "OriginalLanguage" },
                { data: "Overview" },
                { data: "Popularity" },
                { data: "PosterPath" },
                { data: "NumOfUsers" },

            ],
        });

    }
    catch (err) {
        alert(err);
    }
}

function getMsgListSuccessCB(msgList) {
    let str1 = '<section class="page-section  mb-0" > <div class="container">';
    str1 += '<h2 class="page-section-heading text-uppercase text-white">comments</h2>';
    str1 += '<div class="container commentsDiv rounded overflow-auto" style="height:700px;">'
    for (var i = 0; i < msgList.length; i++) {
        //render msg
        str1 += '<div class="row"><div class="panel panel-default">';
        str1 += ' <div class="panel-body" > <section class="post-heading">';
        str1 += '<div class="row"><div class="media">';
        str1 += ' <div class="media-body">';
        str1 += ' <h4 class="media-heading anchor-username">' + msgList[i].UserNamedMsg + '</h4>';
        str1 += '<h6 class="anchor-time">' + msgList[i].Date + '</h6>';
        str1 += '</div></div></div></section></div >';
        str1 += ' <section class="post-body"><p>' + msgList[i].Text + '</p></section>';
        str1 += ' <section class="post-footer"><hr><div class="post-footer-option container">';
        str1 += '<ul class="list-unstyled">';
        str1 += '<li><a class="glyphicon glyphicon-comment" href="#commentSection">' + msgList[i].CommentAmount + '</a></li>';
        str1 += '  <li><a class="glyphicon glyphicon-thumbs-up ">' + msgList[i].LikeAmount + '</a></li>';
        str1 += '   </ul></div>';
        str1 += ' <div class="post-footer-option container">';
        str1 += '<ul class="list-unstyled">';
        if (msgList[i].CurrentUserLike == true) {
            str1 += '<li><button onclick="doLike(this)" id="like_' + i + '" dislikeid="dislike_' + i + '" msg-id="' + msgList[i].MsgID + '" class="btn btn btn-primary likeBTN" style="visibility:hidden;"><i class="glyphicon glyphicon-thumbs-up"></i> Like</button></li>';
            str1 += '<li><button onclick="doDislike(this)" id="dislike_' + i + '" likeid="like_' + i + '" msg-id="' + msgList[i].MsgID + '" class="btn btn btn-danger dislikeBTN"><i class="glyphicon glyphicon-thumbs-down" "></i> Dislike</button></li>';
        }
        else {
            str1 += '<li><button onclick="doLike(this)" id="like_' + i + '" dislikeid="dislike_' + i + '" msg-id="' + msgList[i].MsgID + '" class="btn btn btn-primary likeBTN" ><i class="glyphicon glyphicon-thumbs-up"></i> Like</button></li>';
            str1 += '<li><button onclick="doDislike(this)" id="dislike_' + i + '" likeid="like_' + i + '" msg-id="' + msgList[i].MsgID + '" class="btn btn btn-danger dislikeBTN" style="visibility:hidden;"><i class="glyphicon glyphicon-thumbs-down" "></i> Dislike</button></li>';
        }
        str1 += ' </ul></div>';
        //render comments for msg
        str1 += ' <div id="commentSection_' + i + '" class=" overflow-auto row " ; max-height: 170px; ">';
        for (var j = 0; j < msgList[i].Comments.length; j++) {
            str1 += '<div class="comment col-10">'
            str1 += '<div class="media" >'
            str1 += '<div class="media-body">'
            str1 += '<h4 class="media-heading">' + msgList[i].Comments[j].UserNamedMsg + '</h4>'
            str1 += '<a href="#" class="anchor-time">' + msgList[i].Comments[j].Date + '</a>'
            str1 += '</div></div >'
            str1 += '<section class="comment-body">'
            str1 += '<p>' + msgList[i].Comments[j].Text + '</p></section></div >'

        }
        //render  write commeent for msg area
        str1 += '</div><div class="post-footer-comment-wrapper">';
        str1 += ' <div class="comment-form row">';
        str1 += ' <div class="input-group" id="comment_input">';
        str1 += ' <input type="search" id="input_comment_' + i + '" class="form-control rounded" placeholder="Response to ' + msgList[i].UserNamedMsg + '" aria-label="comment" />';
        str1 += ' <button to-msg="' + msgList[i].MsgID + '" id="' + i + '" class="commentBTN btn btn-outline-primary">comment</button>';
        str1 += ' </div></div></div></section></div></div>';
    }
    //render write  msg area and close div
    str1 += '</div></div></section>'
    str1 += '<div class="container"><h3 class="text-white text-uppercase">add comment</h3>'
    str1 += '<div class="msg-form ]"> <div class="input-group " id="msg_input">'
    str1 += ' <textarea class="form-control rounded" placeholder="Write a comment" name="comments" id="adCommentPlace" style="font-family:sans-serif;font-size:1.2em; margin-right:3%;"></textarea>'
    str1 += '<button type="button" id="AddmsgBTN" class="btn btn-light">comment</button> </div></div></div>'



    document.getElementById("main2").innerHTML = str1;
    //lisiner to comments btn
    $("#AddmsgBTN").click(addComment);
    $(".commentBTN").click(function () { addCommentToComment(this) });


}

function getActorsSuccessCB(creditsList) {
    //render actor list
    let str = '<div class="container">';
    str += '<h2 class="page-section-heading text-uppercase text-white">Actors</h2>'
    str += '<div class="row card_row rounded d-flex flex-row flex-nowrap overflow-auto shadow ">'

    for (var i = 0; i < creditsList.cast.length; i++) {
        let gender = "Female"
        if (creditsList.cast[i].gender == 2) gender = "Male";

        let Actor = {
            ActorId: creditsList.cast[i].id,
            ActorName: creditsList.cast[i].original_name.split("'").join('').split('"').join(''),
            ImgUrl: (imagePath + creditsList.cast[i].profile_path),
            CharacterName: creditsList.cast[i].character.split("'").join('').split('"').join(''),
            Gender: gender,
            SeriesName: currentSeries.name.split("'").join('').split('"').join(''),
            CurrentUserId: currentUserId
        }

        str += '<div class="card card-block mx-2" style="width: 18rem;  background-color:rgba(255, 255, 255,0.8);">'
        str += '<img src="' + Actor.ImgUrl + '" class="card-img-top" alt="..." onerror="imgError(this);">'
        str += '<div class="card-body">'
        str += '<h4>' + Actor.ActorName + '</h4>'
        str += '<p class="card-text">Acting: <br>' + Actor.CharacterName + '</p>';
        str += "<button actor='" + JSON.stringify(Actor) + "' class='btn btn-default swap heartBTN' onclick='addActor(this)'>add to myFavActors  <span class='glyphicon glyphicon-heart-empty'></span> </button>";
        str += '</div></div>';
    }


    str += '</div> </div>';

    document.getElementsByClassName("seriesDiv")[0].innerHTML += str
}

//when user click on add to my favorit actors btn
function addActor(btn) {
    let actor = JSON.parse(btn.getAttribute('actor'));

    console.log(actor);
    let epApi = "../api/Actors";
    ajaxCall("POST", epApi, JSON.stringify(actor), postActorSuccessCB, ErrorCB);

}

function getActorsDBSuccessCB(actorsList) {
    //render favorit actors list
    let str = '<div class="container rounded ">';
    for (var i = 0; i < actorsList.length; i++) {

        str += '<div class="row ActorsDiv rounded text-white">';
        str += ' <div class="left col-3">';
        str += ' <img class=" rounded" src="' + actorsList[i].ImgUrl + '" onerror="imgError(this);"/></div>';
        str += ' <div class="right col-9">';
        str += ' <h2 class="text-uppercase">' + actorsList[i].ActorName + '</h2>';
        str += ' <h4 class="text-uppercase"> acting as:</h4><ul>';

        for (var j = 0; j < actorsList[i].Characters.length; j++) {
            str += '<li>' + actorsList[i].Characters[j] + '</li>'
        }

        str += '</ul>';
        str += '</div ></div > ';

    }
    str += '</div>'

    document.getElementById("main2").innerHTML = '';
    document.getElementById("main").innerHTML = str;

}

function postSeriesSuccessCB() {
    let ep = currentSeason.episodes[currentEPNum];
    let imgURL = imagePath + ep.still_path;
    let episode = {
        EpisodeID: ep.id,
        SeriesID: currentSeries.id,
        UserId: currentUserId,
        SeriesName: currentSeries.name.split("'").join(''),
        EpisodeName: ep.name,
        SeasonNum: ep.season_number,
        ImgURL: imgURL.split("'").join(''),
        Description: ep.overview.split("'").join('').split('"').join('')
    }
    //api call to add the episode to the DB
    let epApi = "../api/Episodes";
    ajaxCall("POST", epApi, JSON.stringify(episode), postEpisodeSuccessCB, ErrorCB);
}

function postEpisodeSuccessCB() {
    alert(" Episode successfully added ")
}




function postActorSuccessCB() {
    alert("actor was added!")
}

function MsgSuccessCB() {
    getMsg(currentSeries.id);

}


function deleteEpisodesSuccessCB(seriesId) {
    getEpisodes(seriesId);
}

function getSeasonErrorCB(err) {
    console.log(err);
    //case season 0 whit TMDB api
    let apiCall = 'https://api.themoviedb.org/3/tv/' + Stv_id + '/season/' + (SseasonNum + 1) + '?api_key=8d1a5ff2a48df63d8f152f0a36a15c7c&language=en-US'
    ajaxCall("GET", apiCall, "", getSeasonSuccessCB, getSeasonErrorCB);
}

//general error call beck function
function ErrorCB(err) {
    console.log(err);

}


//==========live chat  functions=========//

function listenToNewMessages() {
    let before24Hour = new Date().getTime() - (24 * 3600 * 1000);
    ref.on("child_added", snapshot => {
        let msg = {
            name: snapshot.val().name,
            content: snapshot.val().msg,
            date: snapshot.val().date,
        }
        chatMsgArr.push(msg)
        printMessage(msg);

    })
}

function AddMSG() {
    let d = new Date();
    let hour = d.getHours()
    let minutes = d.getMinutes()
    if (hour < 10) hour = "0" + hour;
    if (minutes < 10) minutes = "0" + minutes;
    let datetime = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + "|" + hour + ":" + minutes;

    let msg = $("#msgInput").val()
    let name = currentUser.FirsName + " " + currentUser.LastName;

    ref.push().set({ "msg": msg, "name": name, "date": datetime });
    $("#msgInput").val('');
    $("#msgPlace").animate({ scrollTop: 100000 }, 1000)

}

function printMessage(msg) {
    let str = '<div class="shadow-lg p-3 mb-5 bg-white rounded"><h4>' + msg.name + '</h4><h6>' + msg.date + '</h6><p>' + msg.content + '</p></div>';
    document.getElementById("msgPlace").innerHTML += str;

}

