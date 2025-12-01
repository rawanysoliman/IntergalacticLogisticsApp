import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StarshipInfo } from '../models/starship.model';
import { PlanetInfo } from '../models/planet.model';
import { CreateShipmentRequest, Shipment } from '../models/shipment.model';

@Injectable({
  providedIn: 'root'
})
export class StarshipService {
  private readonly http = inject(HttpClient);
  // Use relative path when served from same origin, or absolute for development
  private readonly apiUrl = '/api';

  getStarships(): Observable<StarshipInfo[]> {
    return this.http.get<StarshipInfo[]>(`${this.apiUrl}/Starships`);
  }

  getStarshipById(id: string): Observable<StarshipInfo> {
    return this.http.get<StarshipInfo>(`${this.apiUrl}/Starships/${id}`);
  }

  getPlanets(): Observable<PlanetInfo[]> {
    return this.http.get<PlanetInfo[]>(`${this.apiUrl}/Planets`);
  }

  getPlanetById(id: string): Observable<PlanetInfo> {
    return this.http.get<PlanetInfo>(`${this.apiUrl}/Planets/${id}`);
  }

  createShipment(request: CreateShipmentRequest): Observable<Shipment> {
    return this.http.post<Shipment>(`${this.apiUrl}/Shipments`, request);
  }

  getShipmentById(id: string): Observable<Shipment> {
    return this.http.get<Shipment>(`${this.apiUrl}/Shipments/${id}`);
  }
}

