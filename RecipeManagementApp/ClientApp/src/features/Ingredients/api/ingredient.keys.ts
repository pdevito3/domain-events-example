const IngredientKeys = {
  all: ['Ingredients'] as const,
  lists: () => [...IngredientKeys.all, 'list'] as const,
  list: (queryParams: string) => [...IngredientKeys.lists(), { queryParams }] as const,
  details: () => [...IngredientKeys.all, 'detail'] as const,
  detail: (id: string) => [...IngredientKeys.details(), id] as const,
}

export { IngredientKeys };
