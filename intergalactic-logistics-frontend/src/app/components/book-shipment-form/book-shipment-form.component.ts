import { Component, signal, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { StarshipService } from '../../services/starship.service';
import { StarshipInfo } from '../../models/starship.model';
import { PlanetInfo } from '../../models/planet.model';
import { CreateShipmentRequest } from '../../models/shipment.model';
import { NotificationComponent, NotificationType } from '../notification/notification.component';
import { catchError, of, forkJoin } from 'rxjs';

@Component({
  selector: 'app-book-shipment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NotificationComponent],
  templateUrl: './book-shipment-form.component.html',
  styleUrl: './book-shipment-form.component.css'
})
export class BookShipmentFormComponent implements OnInit {
  private readonly starshipService = inject(StarshipService);

  starships = signal<StarshipInfo[]>([]);
  planets = signal<PlanetInfo[]>([]);
  loading = signal<boolean>(false);
  submitting = signal<boolean>(false);
  
  notificationMessage = signal<string>('');
  notificationType = signal<NotificationType>('info');
  notificationVisible = signal<boolean>(false);

  shipmentForm = new FormGroup({
    starshipId: new FormControl<string>('', [Validators.required]),
    origin: new FormControl<string>('', [Validators.required]),
    destination: new FormControl<string>('', [Validators.required]),
    cargoWeight: new FormControl<number | null>(null, [Validators.required, Validators.min(0.01), Validators.max(10000)]),
    shippingMethod: new FormControl<string>('StandardSpeed', [Validators.required])
  }, { validators: this.originDestinationValidator() });


  originDestinationValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const group = control as FormGroup;
      const origin = group.get('origin')?.value;
      const destination = group.get('destination')?.value;
      
      if (origin && destination && origin === destination) {
        return { sameOriginDestination: true };
      }
      return null;
    };
  }

  shippingMethods = [
    { value: 'StandardSpeed', label: 'Standard Speed' },
    { value: 'HyperdriveExpress', label: 'Hyperdrive Express' },
    { value: 'SmugglerRoute', label: 'Smuggler Route' }
  ];

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading.set(true);
    
    forkJoin({
      starships: this.starshipService.getStarships().pipe(
        catchError(() => {
          this.showNotification('Failed to load starships', 'error');
          return of([]);
        })
      ),
      planets: this.starshipService.getPlanets().pipe(
        catchError(() => {
          this.showNotification('Failed to load planets', 'error');
          return of([]);
        })
      )
    }).subscribe(({ starships, planets }) => {
      this.starships.set(starships);
      this.planets.set(planets);
      this.loading.set(false);
    });
  }

  onSubmit(): void {
    if (this.shipmentForm.invalid) {
      this.shipmentForm.markAllAsTouched();
      

      if (this.shipmentForm.errors?.['sameOriginDestination']) {
        this.showNotification('Origin and destination cannot be the same', 'error');
      } else if (this.shipmentForm.get('cargoWeight')?.errors?.['min']) {
        this.showNotification('Cargo weight must be 0 or greater', 'error');
      } else {
        this.showNotification('Please fill in all required fields correctly', 'error');
      }
      return;
    }

    this.submitting.set(true);
    this.shipmentForm.disable();
    
    const formValue = this.shipmentForm.value;
    const starship = this.starships().find(s => s.name === formValue.starshipId);
    const starshipName = starship?.name || '';
    
    // Ensure we use swapiId (numeric ID) for backend lookup
    const swapiId = starship?.swapiId || '';


    // console.log('Sending shipment request:', { swapiId, starshipName, shippingMethod: formValue.shippingMethod });

    const request: CreateShipmentRequest = {
      starshipId: swapiId, // Send SWAPI numeric ID for backend lookup
      starshipName,
      origin: formValue.origin || '',
      destination: formValue.destination || '',
      cargoWeight: formValue.cargoWeight || 0,
      shippingMethod: formValue.shippingMethod || 'StandardSpeed'
    };

    this.starshipService.createShipment(request).subscribe({
      next: (shipment) => {
        this.showNotification(`Shipment booked! Cost: ${shipment.cost.toFixed(2)} credits`, 'success');
        this.shipmentForm.reset({ shippingMethod: 'StandardSpeed' });
        this.shipmentForm.enable();
        this.submitting.set(false);
      },
      error: (err) => {
        this.showNotification(err.error?.detail || err.message || 'Failed to book shipment', 'error');
        this.shipmentForm.enable();
        this.submitting.set(false);
      }
    });
  }

  showNotification(message: string, type: NotificationType): void {
    this.notificationMessage.set(message);
    this.notificationType.set(type);
    this.notificationVisible.set(true);
    setTimeout(() => this.notificationVisible.set(false), 5000);
  }

  onNotificationDismiss(): void {
    this.notificationVisible.set(false);
  }
}
