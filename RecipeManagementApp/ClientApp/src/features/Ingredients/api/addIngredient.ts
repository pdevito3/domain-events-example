import { api } from '@/lib/axios';
import { AxiosError } from 'axios';
import { UseMutationOptions, useQueryClient, useMutation } from 'react-query';
import { IngredientKeys } from './ingredient.keys';
import { IngredientDto, IngredientForCreationDto } from '../types';

const addIngredient = (data: IngredientForCreationDto) => {
	return api
		.post('/api/ingredients', data)
		.then((response) => response.data as IngredientDto);
};

export function useAddIngredient(options?: UseMutationOptions<IngredientDto, AxiosError, IngredientForCreationDto>) {
	const queryClient = useQueryClient()

	return useMutation(
		(newIngredient: IngredientForCreationDto) => addIngredient(newIngredient),
		{
			onSuccess: () => {
				queryClient.invalidateQueries(IngredientKeys.lists())
			},
			...options
		});
}
