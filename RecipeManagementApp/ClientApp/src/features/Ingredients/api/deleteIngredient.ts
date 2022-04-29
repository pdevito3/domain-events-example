import { api } from '@/lib/axios';
import { AxiosError } from 'axios';
import { UseMutationOptions, useQueryClient, useMutation } from 'react-query';
import { IngredientKeys } from './ingredient.keys';

async function deleteIngredient(id: string) {
	return api
		.delete(`/api/ingredients/${id}`)
		.then(() => { });
}

export function useDeleteIngredient(options?: UseMutationOptions<void, AxiosError, string>) {
	const queryClient = useQueryClient()

	return useMutation(
		(id: string) => deleteIngredient(id),
		{
			onSuccess: () => {
				queryClient.invalidateQueries(IngredientKeys.lists())
				queryClient.invalidateQueries(IngredientKeys.details())
			},
			...options
		});
}
