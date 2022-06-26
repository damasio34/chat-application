import { ChatMessageComponent } from './chat-message/chat-message.component';
import { Component, ComponentFactoryResolver, ElementRef, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ChatMessage } from './chat-message';
import { ChatService } from './chat.service';
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {

  @ViewChild('chatMessages', { read: ViewContainerRef })
  chatMessages!: ViewContainerRef;

  @ViewChild('messages')
  messages!: ElementRef;

  @ViewChild(ChatMessageComponent, { read: ViewContainerRef })
  chatMessageComponent!: ChatMessageComponent;

  constructor(private chatService: ChatService, private loginService: LoginService, private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
    this.chatService.startConnection((returnedMessage: ChatMessage) => {
      if(!!returnedMessage && returnedMessage.username !== this.loginService.username) {
        this.createNewMessageCard(returnedMessage);
        this.scrollToBottom();
      }
    });
  }

  public sendMessage(chatMessage: ChatMessage): void {
    this.chatService.sendMessage(chatMessage, (returnedMessage: ChatMessage) => {
      this.createNewMessageCard(returnedMessage);
      this.scrollToBottom();
    });
  }

  private scrollToBottom(): void {
    this.messages.nativeElement.scrollTop = this.messages.nativeElement.scrollHeight;
  }

  private createNewMessageCard(chatMessage: ChatMessage) {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ChatMessageComponent);
    const componentRef = this.chatMessages.createComponent<ChatMessageComponent>(componentFactory);
    const compInstance = componentRef.instance;
    compInstance.username = chatMessage.username;
    compInstance.text = chatMessage.text;
  }

}
