import { Component, signal, effect, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StarshipService } from '../../services/starship.service';
import { StarshipInfo } from '../../models/starship.model';
import { StarshipCardComponent } from '../../components/starship-card/starship-card.component';

@Component({
  selector: 'app-starships-page',
  standalone: true,
  imports: [CommonModule, StarshipCardComponent],
  templateUrl: './starships-page.component.html',
  styleUrl: './starships-page.component.css'
})
export class StarshipsPageComponent implements OnInit {
  private readonly starshipService = inject(StarshipService);
  

  starships = signal<StarshipInfo[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  constructor() {

    effect( () => {
      const starshipsList = this.starships();
      if (starshipsList.length > 0) {
        console.log(`Loaded ${starshipsList.length} starships`);
      }
    });
  }

  ngOnInit(): void {
    this.loadStarships();
  }

  loadStarships(): void {
    this.loading.set(true);
    this.error.set(null);

    this.starshipService.getStarships().subscribe({
      next: (Allstarships) => {



        console.log('=== FRONTEND: Starships received from API ===');
        console.log('Total starships:', Allstarships.length);
        if (Allstarships.length > 0) {
          const first = Allstarships[0];
          console.log('First starship FULL object:', first);
          console.log('First starship cargoCapacity:', first.cargoCapacity, 'Type:', typeof first.cargoCapacity);
          console.log('First starship hyperdriveRating:', first.hyperdriveRating, 'Type:', typeof first.hyperdriveRating);
 
        }



        this.starships.set(Allstarships);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || 'Failed to load starships');
        this.loading.set(false);
        console.error('Error loading starships:', err);
      }
    });
  }
}

