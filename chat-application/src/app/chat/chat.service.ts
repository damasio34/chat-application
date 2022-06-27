import { Router } from '@angular/router';
import { LoginService } from 'src/app/login/login.service';
import { ChatMessage } from './chat-message';
import { Injectable } from '@angular/core';

import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { IHttpConnectionOptions } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private hubConnection!: signalR.HubConnection;
  private defaultRoom: string = 'defaultRoom';

  constructor(private loginService: LoginService, private router: Router) { }

  public startConnection = async (onReceiveNewMessage: (chatMessage: ChatMessage) => void) => {
    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => {
        return this.loginService.token;
      }
    };

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.webSocketUrl}/chat`, options)
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
      .catch(err => {
        if(err.toString().includes('401')) {
          this.router.navigate(['/login']);
        }
      });
  }

  public closeConnection = () => this.hubConnection.stop();

  public sendMessage(chatMessage: ChatMessage): void {
    if(chatMessage.room !== this.defaultRoom) {
      this.hubConnection.invoke('RemoveToRoom', this.defaultRoom);
      this.hubConnection.invoke('AddToRoom', chatMessage.room);
      this.defaultRoom = chatMessage.room;
    }

    chatMessage.room = chatMessage.room ?? this.defaultRoom;
    this.hubConnection.invoke('SendMessageToRoom', chatMessage);
  }
}
