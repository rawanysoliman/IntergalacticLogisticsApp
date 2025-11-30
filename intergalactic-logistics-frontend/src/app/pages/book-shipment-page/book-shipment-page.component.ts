import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookShipmentFormComponent } from '../../components/book-shipment-form/book-shipment-form.component';

@Component({
  selector: 'app-book-shipment-page',
  standalone: true,
  imports: [CommonModule, BookShipmentFormComponent],
  templateUrl: './book-shipment-page.component.html',
  styleUrl: './book-shipment-page.component.css'
})
export class BookShipmentPageComponent {
}

