export interface CreateShipmentRequest {
  starshipId: string;
  starshipName: string;
  origin: string;
  destination: string;
  cargoWeight: number;
  shippingMethod: string;
}

export interface Shipment {
  id: string;
  starshipId: string;
  starshipName: string;
  origin: string;
  destination: string;
  cargoWeight: number;
  shippingMethod: string;
  cost: number;
  createdAt: string;
}

