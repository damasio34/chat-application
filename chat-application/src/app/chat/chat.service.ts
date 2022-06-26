import { ChatMessage } from './chat-message';
import { Injectable } from '@angular/core';

import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private hubConnection!: signalR.HubConnection;
  private defaultRoom: string = 'defaultRoom';

  constructor() { }

  public startConnection = async (onReceiveNewMessage: (chatMessage: ChatMessage) => void) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.webSocketUrl}/chat`)
      .configureLogging(signalR.LogLevel.Critical)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.onreconnected((connectionId) => {
      this.hubConnection.invoke('AddToRoom', this.defaultRoom);
    });

    return await this.hubConnection
      .start()
      .then(() => {
        this.hubConnection.invoke('AddToRoom', this.defaultRoom);

        this.hubConnection.on('receiveMessage', (chatMessage: ChatMessage) => {
          onReceiveNewMessage(chatMessage);
        });
      })
      .catch(err => console.error(err));
  }

  public closeConnection = () => this.hubConnection.stop();

  public sendMessage(chatMessage: ChatMessage, callback: (chatMessage: ChatMessage) => void): void {
    chatMessage.room = chatMessage.room ?? this.defaultRoom;
    this.hubConnection.invoke('SendMessageToRoom', chatMessage)
      .then(() => { callback(chatMessage); });
  }
}
