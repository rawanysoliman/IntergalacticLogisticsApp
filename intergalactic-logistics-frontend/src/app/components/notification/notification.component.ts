import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

export type NotificationType = 'success' | 'error' | 'warning' | 'info';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent {
  message = input.required<string>();
  type = input<NotificationType>('info');
  visible = input<boolean>(false);
  
  dismissed = output<void>();

  onDismiss(): void {
    this.dismissed.emit();
  }

}

