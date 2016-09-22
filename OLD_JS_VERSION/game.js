(function()
{
    var requestAnimationFrame = window.requestAnimationFrame || window.mozRequestAnimationFrame || window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;
    window.requestAnimationFrame = requestAnimationFrame;
})();

var canvas = document.getElementById('canvas');
var ctx = canvas.getContext("2d");

// init all the images I need
var sandAnim = ["svg/S1.svg", "svg/S2.svg", "svg/S3.svg", "svg/S4.svg"];
var zapAnim = ["svg/Z2.svg", "svg/Z4.svg", "svg/Z6.svg", "svg/Z8.svg"];
sandFrame = 0;
zapFrame = 0;
gameFrame = 10;
var zapImg = new Image();
zapImg.src = "svg/Z2.svg";
var sandImg = new Image();
sandImg.src = "svg/S1.svg";
var zapDead = new Image();
zapDead.src = "svg/ZD.svg";

// game dimensions
var width = 800, height = 400;
canvas.width = width;
canvas.height = height;

// ZAPLUG character
var player = {
	// positions of the player
	x : 50,
	y : 0,

	// dimensions of my zaplug
	width : 100,
	height : 100,
    speed: 3,
    velX: 0,
    velY: 0,
	jumping: false,
    grounded: false,
	isAlive: true
};

var keys = [],
friction = 0.8,
gravity = 0.2;

// MONSTER
var sandworm = {
	x: 620,
	y: 195,
	width: 50,
	height: 100
}

// collisions with these will kill the player
var monsters = [];
monsters.push(sandworm);

// platforms and walls
var platforms = [];

// wall left
platforms.push({
    x: 0,
    y: 0,
    width: 10,
    height: height
});

// wall right
platforms.push({
    x: 0,
    y: height - 2,
    width: width,
    height: 50
});

// platforms from left to right
platforms.push({
    x: width - 10,
    y: 0,
    width: 50,
    height: height
});
platforms.push({
    x: 40,
    y: 300,
    width: 120,
    height: 35
});
platforms.push({
    x: 175,
    y: 280,
    width: 80,
    height: 35
});
platforms.push({
    x: 220,
    y: 265,
    width: 280,
    height: 25
});
platforms.push({
    x: 490,
    y: 275,
    width: 115,
    height: 25
});
platforms.push({
    x: 600,
    y: 265,
    width: 115,
    height: 25
});

function update(){
	// check keys

	// cannot jump if not on grounded
	// cannot jump if jumping
     if (keys[38] || keys[32]) {
        // up arrow or space
      if(!player.jumping && player.grounded){
       player.jumping = true;
       player.velY = -player.speed*2;
      }
    }
    if (keys[39]) {
        // right arrow
        if (player.velX < player.speed) {
            player.velX++;
         }
    }
    if (keys[37]) {
        // left arrow
        if (player.velX > -player.speed) {
            player.velX--;
        }
    }

    player.velX *= friction;
    player.velY += gravity;

	// collision detect with the platforms
	player.grounded = false;
	for (var i = 0; i < platforms.length; i++) {
		var dir = colCheck(player, platforms[i]);

		// hit the sides of the platform
        if (dir === "l" || dir === "r") {
            player.velX = 0;
            player.jumping = false;
        }

		// dropped onto the platform
		else if (dir === "b") {
            player.grounded = true;
            player.jumping = false;
        }
		//	hit head
		else if (dir === "t") {
            player.velY *= -1;
        }
	}

	// collision detect with the monsters
	for(var i = 0; i < monsters.length; i++)
	{
		var dir = colCheck(player, monsters[i]);

		// if you hit a monster from any side, you're dead
		if (dir === "l" || dir === "r" || dir === "b" || dir === "t")
		{
			player.isAlive = false;
			// change the player animation to dead
			zapImg = zapDead;
		}
	}

	if(player.grounded){
         player.velY = 0;
    }

	// don't move if you're not alive
	if(player.isAlive)
	{
		player.x += player.velX;
		player.y += player.velY;
	}

	// clear the whole context
	ctx.clearRect(0,0,width,height);

	// if we've counted down enough to not twitch, change up the frame
 	if(gameFrame == 0 && player.isAlive)
	{
		gameFrame = 15;
		zapFrame = (zapFrame + 1) % zapAnim.length;
		zapImg.src = zapAnim[zapFrame];
		sandFrame = (zapFrame + 1) % sandAnim.length;
		sandImg.src = sandAnim[sandFrame];
	}
	else
		gameFrame--;

	// draw my funny lil guy
	ctx.drawImage(zapImg, player.x, player.y + 25, player.width, player.height);

	// draw the sandworm
	//ctx.drawImage(sandImg, 620, 195, 50, 100);
	ctx.drawImage(sandImg, sandworm.x, sandworm.y, sandworm.width, sandworm.height);

	ctx.fillStyle = "black";
	ctx.beginPath();
	for(var i = 0; i < platforms.length; i++)
		ctx.rect(platforms[i].x, platforms[i].y, platforms[i].width, platforms[i].height);

	requestAnimationFrame(update);
}

function colCheck(shapeA, shapeB) {
    // get the vectors to check against
    var vX = (shapeA.x + (shapeA.width / 2)) - (shapeB.x + (shapeB.width / 2)),
        vY = (shapeA.y + (shapeA.height / 2)) - (shapeB.y + (shapeB.height / 2)),
        // add the half widths and half heights of the objects
        hWidths = (shapeA.width / 2) + (shapeB.width / 2),
        hHeights = (shapeA.height / 2) + (shapeB.height / 2),
        colDir = null;

    // if the x and y vector are less than the half width or half height, they we must be inside the object, causing a collision
    if (Math.abs(vX) < hWidths && Math.abs(vY) < hHeights)
	{         // figures out on which side we are colliding (top, bottom, left, or right)
		var oX = hWidths - Math.abs(vX), oY = hHeights - Math.abs(vY);
		if (oX >= oY) {
        if (vY > 0) {
				colDir = "t";
				shapeA.y += oY;
            }
			else {
                colDir = "b";
                shapeA.y -= oY;
            }
        }
		else {
            if (vX > 0) {
                colDir = "l";
                shapeA.x += oX;
            } else {
                colDir = "r";
                shapeA.x -= oX;
            }
		}
	}
	return colDir;
}

// get key presses, without jQuery
document.body.addEventListener("keydown", function(e) {
    keys[e.keyCode] = true;
});

document.body.addEventListener("keyup", function(e) {
    keys[e.keyCode] = false;
});

window.addEventListener("load",function(){
    update();
});
