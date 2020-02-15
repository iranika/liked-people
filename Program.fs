open canopy.runner.classic
open canopy.classic
open canopy.types
open canopy.configuration
open OpenQA.Selenium
open FSharp.Data
open System
open System.IO
open System.Runtime.Serialization.Json
open System.Runtime.Serialization
open Newtonsoft.Json

let id = Environment.GetEnvironmentVariable("TWITTER_ID")
let pass = Environment.GetEnvironmentVariable("TWITTER_PASSWD")
printfn "%s" id
printfn "%s" pass
canopy.configuration.chromeDir <- "."
//start an instance of chrome
start ChromeHeadless
//go to tweetdeck
url "https://tweetdeck.twitter.com/"
sleep 3
//login
click "Log in"
sleep 5

"#react-root > div > div > div.css-1dbjc4n.r-1pi2tsx.r-13qz1uu.r-417010 > main > div > div > form > div > div:nth-child(6) > label > div.css-1dbjc4n.r-18u37iz.r-16y2uox.r-1wbh5a2.r-1udh08x > div > input" << id
"#react-root > div > div > div.css-1dbjc4n.r-1pi2tsx.r-13qz1uu.r-417010 > main > div > div > form > div > div:nth-child(7) > label > div.css-1dbjc4n.r-18u37iz.r-16y2uox.r-1wbh5a2.r-1udh08x > div > input" << pass
click "#react-root > div > div > div.css-1dbjc4n.r-1pi2tsx.r-13qz1uu.r-417010 > main > div > div > form > div > div:nth-child(8) > div > div"

//click いいねコレクションの一番上
sleep 3
click "#container > div > section:nth-child(1) > div > div:nth-child(1) > div.js-column-content.column-content.flex-auto.position-rel.flex.flex-column.height-p--100 > div.js-column-scroller.js-dropdown-container.column-scroller.position-rel.scroll-v.flex-auto.height-p--100.scroll-styled-v > div > article:nth-child(1) > div > div > div.tweet-body.js-tweet-body > p"
//そのツイートのいいねしたユーザ一覧
sleep 3
click "#container > div > section.js-column.column.will-animate.is-shifted-1.js-column-state-detail-view > div > div.js-column-detail.column-detail.column-panel.flex.flex-column.height-p--100 > div > div > div > div.js-tweet-detail.tweet-detail-wrapper > article > div > div.js-tweet.tweet-detail > footer > div > div.js-stats-list.tweet-stats.flex.flex-row.flex-align--baseline.flex-wrap--wrap > div:nth-child(3) > span"
//名前とiconの取得
type GuysRecord ={
    name: string
    icon: string
}
let LikedGuys =
    elements "#container > div > section.js-column.column.will-animate.is-shifted-2.js-column-state-social-proof > div > div.js-column-social-proof.column-detail-level-2.column-panel.flex.flex-column.height-p--100 > div > div > article > div > div"
    |> List.map (fun elem -> {name=(elem |> elementWithin ".fullname" |> read); icon=(elem |> elementWithin ".tweet-avatar").GetAttribute("src");})

//Jsonとして保存
printfn "%O" LikedGuys
File.WriteAllText("Liked.json", JsonConvert.SerializeObject(LikedGuys))
quit()