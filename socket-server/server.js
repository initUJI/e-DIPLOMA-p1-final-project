import { Server } from "socket.io";
import express from "express";

const app = express().listen(process.env.PORT, () => {
  console.log(`Server running on http://localhost:${process.env.PORT}`);
});

const io = new Server(app, {
  cors: {
    origin: "*",
  },
});

io.on("connection", (socket) => {
    console.log("a user connected");

    socket.on("user-one-play", (msg) => {
        console.log("User one played: ", msg);
        io.emit("user-two-move", msg);
    });

    socket.on("user-two-play", (msg) => {
        console.log("User two played: ", msg);
        io.emit("user-one-move", msg);
    });

    socket.on("disconnect", () => {
        console.log("a user disconnected");
    });
});
// Unity code: https://www.youtube.com/watch?v=LR3-xJLKtbE