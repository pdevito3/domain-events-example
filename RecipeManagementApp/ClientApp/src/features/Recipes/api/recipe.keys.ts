const RecipeKeys = {
  all: ['Recipes'] as const,
  lists: () => [...RecipeKeys.all, 'list'] as const,
  list: (queryParams: string) => [...RecipeKeys.lists(), { queryParams }] as const,
  details: () => [...RecipeKeys.all, 'detail'] as const,
  detail: (id: string) => [...RecipeKeys.details(), id] as const,
}

export { RecipeKeys };
