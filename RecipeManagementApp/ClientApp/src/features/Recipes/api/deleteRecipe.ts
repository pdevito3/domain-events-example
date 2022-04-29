import { api } from '@/lib/axios';
import { AxiosError } from 'axios';
import { UseMutationOptions, useQueryClient, useMutation } from 'react-query';
import { RecipeKeys } from './recipe.keys';

async function deleteRecipe(id: string) {
	return api
		.delete(`/api/recipes/${id}`)
		.then(() => { });
}

export function useDeleteRecipe(options?: UseMutationOptions<void, AxiosError, string>) {
	const queryClient = useQueryClient()

	return useMutation(
		(id: string) => deleteRecipe(id),
		{
			onSuccess: () => {
				queryClient.invalidateQueries(RecipeKeys.lists())
				queryClient.invalidateQueries(RecipeKeys.details())
			},
			...options
		});
}
