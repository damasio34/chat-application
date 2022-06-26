import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatMessageSenderComponent } from './chat-message-sender.component';

describe('ChatMessageSenderComponent', () => {
  let component: ChatMessageSenderComponent;
  let fixture: ComponentFixture<ChatMessageSenderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChatMessageSenderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatMessageSenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
