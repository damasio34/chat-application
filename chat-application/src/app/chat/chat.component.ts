import { ChatMessageComponent } from './chat-message/chat-message.component';
import { Component, ComponentFactoryResolver, ElementRef, ViewChild, ViewContainerRef } from '@angular/core';
import { ChatMessage } from './chat-message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent {

  @ViewChild('chatMessages', { read: ViewContainerRef })
  chatMessages!: ViewContainerRef;

  @ViewChild('messages')
  messages!: ElementRef;

  @ViewChild(ChatMessageComponent, { read: ViewContainerRef })
  chatMessageComponent!: ChatMessageComponent;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  public sendMessage(chatMessage: ChatMessage): void {
    this.createNewMessageCard(chatMessage);
    this.scrollToBottom();
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
