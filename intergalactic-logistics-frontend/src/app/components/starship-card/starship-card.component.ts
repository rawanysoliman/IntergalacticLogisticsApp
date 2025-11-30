import { Component, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StarshipInfo } from '../../models/starship.model';

@Component({
  selector: 'app-starship-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './starship-card.component.html',
  styleUrl: './starship-card.component.css'
})
export class StarshipCardComponent {
  starship = input.required<StarshipInfo>();

  constructor() {
    effect(() => {
      const ship = this.starship();
      console.log(`[StarshipCard] ${ship.name}:`, {
        cargoCapacity: ship.cargoCapacity,
        hyperdriveRating: ship.hyperdriveRating,
        starshipClass: ship.starshipClass
      });
    });
  }
}

