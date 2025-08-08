# ROULETTE GAME

This is a fully functional roulette game for Joker Games case project.

## How To Play
The game has the exactly same rules with the classic European Roulette Game. When the game starts, you'll see the roulette table with the wheel. You have red roulette chip that you can move around. Simply drag and drop it onto the roulette numbers. Based on your placement and roulette rules, the selected numbers will be highlighted.

You can choose the amount of your bet by clicking left and right arrow icons.

You can "Cheat" or simply test the game integrity using the cheat input field at right-bottom corner. Entering a number there will force the ball to land onto that number when you spin.

When you're ready, simply click on "BET!" button and wait for the ball animation to end.

# Technical Background
## Mechanics
The game consists of two main mechanic: **Betting** and **Spinning**. Betting phase encapsulates mechanics like *dragging player chip*, detecting the *bet type* (inner and outer bets), changing your bets and calculating your potential *payout* based on the bet. Spinning phase encapsulates the spinning mechanic of the ball based on the given value, which means the value is *determined* before the ball lands on the seemingly random number. It also contains the calculations regarding the realistic spin. Since the value is deterministic, we can't rely on Unity's own physics since It will be difficult to understand where the ball is going to land. Instead, a simple spiral movement is emulated during the spin.
### Betting
The main problem with bet detection is, there are too many possibilities to determine. It's possible to add detector object between and on every number, and every outer bet, but far from being effective solution. A more elegant approach is creating [Bet Areas](https://github.com/dogabeey/roulette-game/blob/main/Assets/Scripts/Bet%20Areas/BetArea.cs) for every bet type.

In [BetArea.cs](https://github.com/dogabeey/roulette-game/blob/main/Assets/Scripts/Bet%20Areas), we can set the spanning area of a bet type, and introduce a child class for each Bet Type. e.g For the inner bets, a [NumberBetsArea.cs](https://github.com/dogabeey/roulette-game/blob/main/Assets/Scripts/Bet%20Areas/NumbersBetArea.cs) class is written. This class calculates the player chip's coordinates relative to the bet area's starting and ending references. Other inherited classes work similarly. BetArea.cs also holds information like how much the payout will be, based on the numbers amount (if inner bet) or bet area type (if outer bet).
### Spinning
Spinning of the ball works with a very simple principle of creating a spiral. The ball makes two basic movement: Moving towards the center, and moving around the center. These two basic movement creates the illusion that the ball is spiraling towards the number holes in the wheel.
To determine the number It'll land, the ball simply moves around without dropping for a certain amount of turns. It stops once reaches the angle It's supposed to land. This angle is determined a pre-made list of integers and their order. For the next step, the ball rotates around the center once more but It also gets closer to the wheel center at the same time. Since It rotates by 360 degrees, the angular position doesn't change and the ball lands exactly where we want it to!
A small issue is the wheel movement. Since the wheel moves with a slower and reversed speed of the ball, the ball lands in wrong number if doesn't take it into the account. To solve this, we simply shift the numbers by the multiplication of angular wheel speed and total spinning duration. Then we calculate how many numbers the wheel shifted since the ball started spinning.



