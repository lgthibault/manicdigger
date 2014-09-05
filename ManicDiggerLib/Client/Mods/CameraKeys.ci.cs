﻿public class ModCameraKeys : ClientMod
{
    public override void OnNewFrameFixed(Game game, NewFrameEventArgs args)
    {
        float one = 1;
        float dt = args.GetDt();

        if (game.guistate == GuiState.MapLoading) { return; }

        bool angleup = false;
        bool angledown = false;
        float overheadcameraanglemovearea = one * 5 / 100;
        float overheadcameraspeed = 3;
        game.wantsjump = game.GuiTyping == TypingState.None && game.keyboardState[game.GetKey(GlKeys.Space)];
        game.shiftkeydown = game.keyboardState[game.GetKey(GlKeys.ShiftLeft)];
        game.movedx = 0;
        game.movedy = 0;
        game.moveup = false;
        game.movedown = false;
        if (game.guistate == GuiState.Normal)
        {
            if (game.GuiTyping == TypingState.None)
            {
                if (game.d_Physics.reachedwall_1blockhigh && (game.AutoJumpEnabled || (!game.platform.IsMousePointerLocked())))
                {
                    game.wantsjump = true;
                }
                if (game.overheadcamera)
                {
                    CameraMove m = new CameraMove();
                    if (game.keyboardState[game.GetKey(GlKeys.A)]) { game.overheadcameraK.TurnRight(dt * overheadcameraspeed); }
                    if (game.keyboardState[game.GetKey(GlKeys.D)]) { game.overheadcameraK.TurnLeft(dt * overheadcameraspeed); }
                    if (game.keyboardState[game.GetKey(GlKeys.W)]) { angleup = true; }
                    if (game.keyboardState[game.GetKey(GlKeys.S)]) { angledown = true; }
                    game.overheadcameraK.Center.X = game.player.playerposition.X;
                    game.overheadcameraK.Center.Y = game.player.playerposition.Y;
                    game.overheadcameraK.Center.Z = game.player.playerposition.Z;
                    m.Distance = game.overheadcameradistance;
                    m.AngleUp = angleup;
                    m.AngleDown = angledown;
                    game.overheadcameraK.Move(m, dt);
                    float toDest = game.Dist(game.player.playerposition.X, game.player.playerposition.Y, game.player.playerposition.Z,
                    game.playerdestination.X + one / 2, game.playerdestination.Y - one / 2, game.playerdestination.Z + one / 2);
                    if (toDest >= 1)
                    {
                        game.movedy += 1;
                        if (game.d_Physics.reachedwall)
                        {
                            game.wantsjump = true;
                        }
                        //player orientation
                        float qX = game.playerdestination.X - game.player.playerposition.X;
                        float qY = game.playerdestination.Y - game.player.playerposition.Y;
                        float qZ = game.playerdestination.Z - game.player.playerposition.Z;
                        float angle = game.VectorAngleGet(qX, qY, qZ);
                        game.player.playerorientation.Y = Game.GetPi() / 2 + angle;
                        game.player.playerorientation.X = Game.GetPi();
                    }
                }
                else if (game.enable_move)
                {
                    if (game.keyboardState[game.GetKey(GlKeys.W)]) { game.movedy += 1; }
                    if (game.keyboardState[game.GetKey(GlKeys.S)]) { game.movedy += -1; }
                    if (game.keyboardState[game.GetKey(GlKeys.A)]) { game.movedx += -1; game.localplayeranimationhint.leanleft = true; game.localstance = 1; }
                    else { game.localplayeranimationhint.leanleft = false; }
                    if (game.keyboardState[game.GetKey(GlKeys.D)]) { game.movedx += 1; game.localplayeranimationhint.leanright = true; game.localstance = 2; }
                    else { game.localplayeranimationhint.leanright = false; }
                    if (!game.localplayeranimationhint.leanleft && !game.localplayeranimationhint.leanright) { game.localstance = 0; }

                    game.movedx += game.touchMoveDx;
                    game.movedy += game.touchMoveDy;
                }
            }
            if (game.ENABLE_FREEMOVE || game.Swimming())
            {
                if (game.GuiTyping == TypingState.None && game.keyboardState[game.GetKey(GlKeys.Space)])
                {
                    game.moveup = true;
                }
                if (game.GuiTyping == TypingState.None && game.keyboardState[game.GetKey(GlKeys.ControlLeft)])
                {
                    game.movedown = true;
                }
            }
        }
    }
}