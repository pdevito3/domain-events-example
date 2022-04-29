import { api } from '@/lib/axios';
import { AxiosError } from 'axios';
import { UseMutationOptions, useQueryClient, useMutation } from 'react-query';
import { RecipeKeys } from './recipe.keys';
import { RecipeDto, RecipeForCreationDto } from '../types';

const addRecipe = (data: RecipeForCreationDto) => {
	return api
		.post('/api/recipes', data)
		.then((response) => response.data as RecipeDto);
};

export function useAddRecipe(options?: UseMutationOptions<RecipeDto, AxiosError, RecipeForCreationDto>) {
	const queryClient = useQueryClient()

	return useMutation(
		(newRecipe: RecipeForCreationDto) => addRecipe(newRecipe),
		{
			onSuccess: () => {
				queryClient.invalidateQueries(RecipeKeys.lists())
			},
			...options
		});
}
