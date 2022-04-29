import { api } from '@/lib/axios';
import { AxiosError } from 'axios';
import { UseMutationOptions, useQueryClient, useMutation } from 'react-query';
import { RecipeKeys } from './recipe.keys';
import { RecipeForUpdateDto } from '../types';

export const updateRecipe = (id: string, data: RecipeForUpdateDto) => {
	return api
		.put(`/api/recipes/${id}`, data)
		.then(() => { });
};

export function useUpdateRecipe(id: string, options?: UseMutationOptions<void, AxiosError, RecipeForUpdateDto>) {
	const queryClient = useQueryClient()

	return useMutation(
		(updatedRecipe: RecipeForUpdateDto) => updateRecipe(id, updatedRecipe),
		{
			onSuccess: () => {
				queryClient.invalidateQueries(RecipeKeys.lists())
				queryClient.invalidateQueries(RecipeKeys.details())
			},
			...options
		});
}
