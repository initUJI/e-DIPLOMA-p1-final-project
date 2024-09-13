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


const userRooms = {};
let currentRoom = 1;
const pairedRooms = {};
const userSockets = {};
let connectedUsers = 0;

// Opción 1 - Dos usuarios se conectan a la misma sala.
// Cada vez que un usuario emita el mensaje play, el otro de la misma recibirá el mensaje move.
// No hay limite de salas. Solo hay dos usuarios por sala. No hay limite de usuarios.
/* io.on("connection", (socket) => {
  const user = socket.handshake.query.user;
  let room = `room-${currentRoom}`;
  const roomUsers = io.sockets.adapter.rooms.get(room)?.size || 0;
  if (roomUsers >= process.env.MAX_USERS_ROOM) {
    currentRoom++;
  }
  room = `room-${currentRoom}`;
  userRooms[user] = room;
  socket.join(room);
  socket.emit("joined-room", {
    room: userRooms[user],
    user: roomUsers + 1,
  });
  
  console.log(`User ${user} connected to room ${room}`);

  socket.on("play", (msg) => {
    console.log(`User ${user} played ${msg}`);
    //socket.broadcast.to(userRooms[user]).emit("move", msg);
    io.to(userRooms[user]).emit("move", msg);
  });

  socket.on("disconnect", () => {
    console.log(`User ${user} disconnected from room ${room}`);
    if (!io.sockets.adapter.rooms.get(room)) {
      delete userRooms[user];
      console.log(`Room ${room} deleted`);
    }
  });
}); */

// Opción 1.2 - Dos usuarios se conectan a la misma sala.
// Cada vez que un usuario emita el mensaje play, el otro de la misma recibirá el mensaje move.
// Cuando los dos hayan jugado, se enviará un mensaje de ejecución.
// No hay limite de salas. Solo hay dos usuarios por sala. No hay limite de usuarios.
/* io.on("connection", (socket) => {
  const user = socket.handshake.query.user;
  let room = `room-${currentRoom}`;
  const roomUsers = io.sockets.adapter.rooms.get(room)?.size || 0;
  if (roomUsers >= process.env.MAX_USERS_ROOM) {
    currentRoom++;
  }
  room = `room-${currentRoom}`;
  userRooms[user] = room;
  userPlayed[user] = false;
  socket.join(room);
  socket.emit("joined-room", room);
  
  console.log(`User ${user} connected to room ${room}`);

  socket.on("play", (msg) => {
    userPlayed[user] = true;
    socket.broadcast.to(userRooms[user]).emit("move", msg);

    if (Object.keys(userPlayed).every((u) => userPlayed[u])) {
      io.to(userRooms[user]).emit("execute");
      userPlayed[Object.keys(userPlayed)[0]] = false;
      userPlayed[Object.keys(userPlayed)[1]] = false;
    }
  });

  socket.on("disconnect", () => {
    console.log(`User ${user} disconnected from room ${room}`);
    if (!io.sockets.adapter.rooms.get(room)) {
      delete userRooms[user];
      console.log(`Room ${room} deleted`);
    }
  });
}); */

// Opción 1.3 - Dos usuarios se conectan a la misma sala.
// Cada vez que un usuario emita el mensaje play, el otro de la misma recibirá el mensaje move.
// Cuando los emitan el mensaje play, se enviará un mensaje de ejecución.
// No hay limite de salas. Solo hay dos usuarios por sala. No hay limite de usuarios.
io.on("connection", (socket) => {
  const user = socket.handshake.query.user;
  let room = `room-${currentRoom}`;
  const roomUsers = io.sockets.adapter.rooms.get(room)?.size || 0;
  if (roomUsers >= process.env.MAX_USERS_ROOM) {
    currentRoom++;
  }
  room = `room-${currentRoom}`;
  userRooms[user] = room;
  userPlayed[user] = false;
  socket.join(room);
  socket.emit("joined-room", room);
  
  console.log(`User ${user} connected to room ${room}`);

  socket.on("update", (msg) => {
    socket.to(userRooms[user]).emit("move", msg);
  });

  socket.on("ready", () => {
    userPlayed[user] = true;
    if (Object.keys(userPlayed).every((u) => userPlayed[u])) {
      io.to(userRooms[user]).emit("execute");
      userPlayed[Object.keys(userPlayed)[0]] = false;
      userPlayed[Object.keys(userPlayed)[1]] = false;
    }
  });

  socket.on("disconnect", () => {
    console.log(`User ${user} disconnected from room ${room}`);
    if (!io.sockets.adapter.rooms.get(room)) {
      delete userRooms[user];
      console.log(`Room ${room} deleted`);
    }
  });
});

// Opción 2 - Cada usuario se conecta a una sala distinta.
// Cada vez que un usuario emita el mensaje play, se enviará a la sala contraria.
// Solo hay dos salas. Solo hay un usuario por sala. No hay limite de usuarios.
/* io.on("connection", (socket) => {
  const user = socket.handshake.query.user;
  const room = `room-${currentRoom}`;
  userRooms[user] = room;
  socket.join(room);

  console.log(`User ${user} connected to room ${room}`);

  if (currentRoom % 2 === 0) {
    pairedRooms[`room-${currentRoom - 1}`] = `room-${currentRoom}`;
    pairedRooms[`room-${currentRoom}`] = `room-${currentRoom - 1}`;

    console.log(`Paired rooms: ${pairedRooms[`room-${currentRoom - 1}`]} - ${pairedRooms[`room-${currentRoom}`]}`);
  }
  currentRoom++;

  socket.on("play", (msg) => {
    const targetRoom = pairedRooms[userRooms[user]];
    io.to(targetRoom).emit("move", msg);
  });

  socket.on("disconnect", () => {
    console.log(`User ${user} disconnected`);
  });
}); */

// Opción 3 - Solo hay dos usuarios conectados al servidor.
// Cada vez que un usuario emita el mensaje play, se enviará al otro usuario.
// No hay salas. Solo hay dos usuarios.
/* io.on("connection", (socket) => {
  if (connectedUsers >= process.env.MAX_USERS_TOTAL) {
    socket.emit("full", "Server is full");
    socket.disconnect();
    return;
  }

  const user = socket.handshake.query.user;
  userSockets[user] = socket;
  connectedUsers++;
  console.log(`User ${user} connected`);

  socket.on("play", (msg) => {
    const targetUser = Object.keys(userSockets).find((u) => u !== user);
    userSockets[targetUser].emit("move", msg);
  });

  socket.on("disconnect", () => {
    connectedUsers--;
    delete userSockets[user];
    console.log(`User ${user} disconnected`);
  });
}); */