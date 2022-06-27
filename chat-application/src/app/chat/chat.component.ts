import { ChatMessageComponent } from './chat-message/chat-message.component';
import { Component, ComponentFactoryResolver, ComponentRef, ElementRef, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ChatMessage } from './chat-message';
import { ChatService } from './chat.service';

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

  private chatMessageComponents: Array<ComponentRef<ChatMessageComponent>> = [];
  private _limitOfMessages: number = 50; // ToDo: Get From Environment

  constructor(private chatService: ChatService, private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
    this.chatService.startConnection((returnedMessage: ChatMessage) => {
      this.createNewMessageCard(returnedMessage);

      if (this.chatMessageComponents.length > this._limitOfMessages) {
        var componentRef = this.chatMessageComponents.shift();
        componentRef?.destroy();
      }

      this.scrollToBottom();
    });
  }

  public sendMessage(chatMessage: ChatMessage): void {
    this.chatService.sendMessage(chatMessage);
  }

  private scrollToBottom(): void {
    this.messages.nativeElement.scrollTop = this.messages.nativeElement.scrollHeight;
  }

  private createNewMessageCard(chatMessage: ChatMessage) {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(ChatMessageComponent);
    const componentRef = this.chatMessages.createComponent<ChatMessageComponent>(componentFactory);
    const componentInstance = componentRef.instance;
    componentInstance.username = chatMessage.username;
    componentInstance.text = chatMessage.text;
    componentInstance.room = chatMessage.room;

    this.chatMessageComponents.push(componentRef);
  }

}
