export interface QueryParams {
  pageNumber?: number;
  pageSize?: number;
  filters?: string;
  sortOrder?: string;
}

export interface IngredientDto {
  id: string;
  Name: string;
  Quantity: string;
  Measure: string;
  RecipeId: string;
}

export interface IngredientForManipulationDto {
  id: string;
  Name: string;
  Quantity: string;
  Measure: string;
  RecipeId: string;
}

export interface IngredientForCreationDto extends IngredientForManipulationDto { }
export interface IngredientForUpdateDto extends IngredientForManipulationDto { }

// need a string enum list?
// const StatusList = ['Status1', 'Status2', null] as const;
// export type Status = typeof StatusList[number];
// Then use as --> status: Status;
